using FuzzyString;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Comparer
{
    public class TextToleranceFactory
    {
        public TextTolerance Instantiate(string value)
        {
            //Empty string equals no tolerance
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return TextTolerance.None;

            value = value.Trim().Replace(" ", "");

            //extract the value between parenthesis
            var distanceString = Regex.Match(value, @"\(([^)]*)\)").Groups[1].Value;
            var isDistanceNumeric = Double.TryParse(distanceString, NumberStyles.Float, CultureInfo.InvariantCulture, out var distanceNumeric);
            var distanceEnum = Enum.GetNames(typeof(FuzzyStringComparisonTolerance)).SingleOrDefault(x => x.ToLower() == distanceString.ToLower());
            if (string.IsNullOrEmpty(distanceEnum) && !isDistanceNumeric)
                throw new ArgumentException($"The value of the distance/coefficient for a text tolerance must be a numeric value or the specific values weak/normal/strong. The value '{distanceString}' is not.");

            //extract the name of the tolerance
            var names = value.Split(new[] { '(' })[0].Replace("-", "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (names.Count() == 0)
                throw new ArgumentException($"You must specify at least one method for the text tolerance.");
            if (names.Count() > 1 && isDistanceNumeric)
                throw new ArgumentException($"You cannot specify an exact value when more than one method is specified for the text tolerance.");

            if (names.Count() == 1)
            {
                if (!FindFuzzyMethod(names[0], out var correctName, out var func))
                    throw new ArgumentException($"The method '{names[0]}' is not supported for a text tolerance.");

                var predicate = FindCorrectPredicate(correctName);

                var readableName = Regex.Replace(correctName, "([a-z])([A-Z])", "$1 $2").ToLower();
                readableName = readableName.First().ToString().ToUpper() + readableName.Substring(1);
                return new TextSingleMethodTolerance(readableName, distanceNumeric, func, predicate);
            }
            else
            {
                var options = new List<FuzzyStringComparisonOptions>();
                var readableNames = new List<string>();
                foreach (var name in names)
                {
                    if (!FindFuzzyEnum(name, out var correctName, out var correctValue))
                        throw new ArgumentException($"The method '{name}' is not supported for a text tolerance.");
                    options.Add(correctValue);
                    readableNames.Add(correctName);
                }
                var tolerance = (FuzzyStringComparisonTolerance)Enum.Parse(typeof(FuzzyStringComparisonTolerance), distanceEnum);
                Func<string, string, bool> implementation = (x, y) => x.ApproximatelyEquals(y, options, tolerance);
                return new TextMultipleMethodsTolerance(string.Join(", ", readableNames), distanceEnum, implementation);
            }

        }

        private Func<double, double, bool> FindCorrectPredicate(string correctName)
        {
            if (correctName.EndsWith("Coefficient"))
                return (x, y) => x >= y;
            else
                return (x, y) => x <= y;
        }

        protected bool FindFuzzyMethod(string name, out string correctName, out Func<string, string, double> func)
        {
            func = null;
            correctName = string.Empty;

            var type = typeof(FuzzyString.ComparisonMetrics);
            var names = type.GetMethods().Select(x => x.Name);
            if (names.Contains(name, StringComparer.InvariantCultureIgnoreCase))
            {
                correctName = names.Single(x => StringComparer.InvariantCultureIgnoreCase.Compare(name, x) == 0);
                func = GetMethod(type, correctName);
            }
            else if (names.Contains(name + "Distance", StringComparer.InvariantCultureIgnoreCase))
            {
                correctName = names.Single(x => StringComparer.InvariantCultureIgnoreCase.Compare(name + "Distance", x) == 0);
                func = GetMethod(type, correctName);
            }
            else if (names.Count(x => x.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)) == 1)
            {
                correctName = names.Single(x => x.StartsWith(name, StringComparison.InvariantCultureIgnoreCase));
                func = GetMethod(type, correctName);
            }
            else
                return false;

            return true;
        }

        protected bool FindFuzzyEnum(string name, out string correctName, out FuzzyStringComparisonOptions correctValue)
        {
            name = name.StartsWith("Use") ? name : "Use" + name;
            correctValue = 0;
            correctName = string.Empty;
            var type = typeof(FuzzyStringComparisonOptions);
            var names = Enum.GetNames(type);
            if (names.Contains(name, StringComparer.InvariantCultureIgnoreCase))
                correctName = names.Single(x => StringComparer.InvariantCultureIgnoreCase.Compare(name, x) == 0);
            else if (names.Contains(name + "Distance", StringComparer.InvariantCultureIgnoreCase))
                correctName = names.Single(x => StringComparer.InvariantCultureIgnoreCase.Compare(name + "Distance", x) == 0);
            else if (names.Count(x => x.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)) == 1)
                correctName = names.Single(x => x.StartsWith(name, StringComparison.InvariantCultureIgnoreCase));
            else
                return false;

             correctValue = (FuzzyStringComparisonOptions)Enum.Parse(type, correctName);

            return true;
        }

        protected Func<string, string, double> GetMethod(Type type, string methodName)
        {
            var methodInfo = type.GetMethod(methodName);
            if (methodInfo.ReturnType == typeof(double))
                return (Func<string, string, double>)methodInfo.CreateDelegate(typeof(Func<string, string, double>));
            else
            {
                var firstDelegate = methodInfo.CreateDelegate(typeof(Func<string, string, Int32>));
                Func<string, string, double> convert = (x, y) => { return Convert.ToDouble(firstDelegate.DynamicInvoke(new[] { x, y })); };
                return convert;
            }
        }
    }
}
