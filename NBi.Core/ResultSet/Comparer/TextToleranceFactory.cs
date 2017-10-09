using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Comparer
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
            if (!Double.TryParse(distanceString, NumberStyles.Float, CultureInfo.InvariantCulture, out var distance))
                throw new ArgumentException($"The value of the distance/coefficient for a text tolerance must be a numeric value and the value '{distanceString}' is not.");

            //extract the name of the tolerance
            var name = value.Split(new[] { '(' })[0].Replace("-", "");
            if (!FindFuzzyMethod(name, out var correctName, out var func))
                throw new ArgumentException($"The method '{name}' is not supported for a text tolerance.");

            var predicate = FindCorrectPredicate(correctName);

            var readableName = Regex.Replace(correctName, "([a-z])([A-Z])", "$1 $2").ToLower();
            readableName = readableName.First().ToString().ToUpper() + readableName.Substring(1);
            return new TextTolerance(readableName, distance, func, predicate);
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
