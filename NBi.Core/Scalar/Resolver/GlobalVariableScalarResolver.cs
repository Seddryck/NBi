using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    class GlobalVariableScalarResolver<T> : IScalarResolver<T>
    {
        private readonly GlobalVariableScalarResolverArgs args;

        public GlobalVariableScalarResolver(GlobalVariableScalarResolverArgs args)
        {
            this.args = args;
        }

        public GlobalVariableScalarResolver(string name, IDictionary<string, ITestVariable> variables)
        {
            this.args = new GlobalVariableScalarResolverArgs(name, variables);
        }

        public T Execute()
        {
            CheckVariableExists(args.VariableName, args.GlobalVariables);
            var evaluation = EvaluateVariable(args.GlobalVariables[args.VariableName]);
            var typedEvaluation = StrongTypingVariable(evaluation);
            DisplayVariable(args.VariableName, typedEvaluation);

            return (T)typedEvaluation;
        }

        object IScalarResolver.Execute() => Execute();

        private void DisplayVariable(string name, object value)
        {
            var invariantCulture = new CultureFactory().Invariant;
            var msg = $@"Variable '{args.VariableName}' used with value: {
                    (
                        value == null ? "(null)" :
                        value is string && string.IsNullOrEmpty(value.ToString()) ? "(empty)" :
                        value
                    )
                }
            ";
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, msg.ToString(invariantCulture));
        }

        private static object StrongTypingVariable(object input)
        {
            IFormatProvider formatProvider =  typeof(T) == typeof(DateTime) 
                ? (IFormatProvider) System.Globalization.DateTimeFormatInfo.InvariantInfo
                : System.Globalization.NumberFormatInfo.InvariantInfo;

            if (input != null && input.ToString().EndsWith("%"))
                input = input.ToString().Substring(0, input.ToString().Length - 1);

            var output = Convert.ChangeType(input, typeof(T), formatProvider);
            return output;
        }

        private void CheckVariableExists(string name, IDictionary<string, ITestVariable> variables)
        {
            if (!variables.ContainsKey(name))
            {
                var caseIssues = variables.Keys.Where(k => String.Equals(k, name, StringComparison.OrdinalIgnoreCase));

                if (caseIssues.Count() > 0)
                    throw new NBiException($"The variable named '{name}' is not defined. Pay attention, variables are case-sensitive. Did you mean '{string.Join("' or '", caseIssues)}'?");

                var arobaseIssues = variables.Keys.Where(k => String.Equals(k.Replace("@", string.Empty), name, StringComparison.OrdinalIgnoreCase));
                if (arobaseIssues.Count() > 0)
                    throw new NBiException($"The variable named '{name}' is not defined. Pay attention, variables shouldn't begin with an arobase (@). Consider to review the name of the following variable{(arobaseIssues.Count() == 1 ? string.Empty : "s")}: '{(string.Join("' and '", arobaseIssues))}' at the top of your test-suite.");

                var countMsg =
                    variables.Count() == 0 ? "No variables are" :
                    variables.Count() == 1 ? $"1 variable '{(variables).Keys.ElementAt(0)}' is"
                    : $"{variables.Count()} other variables are";

                throw new NBiException($"The variable named '{name}' is not defined. {countMsg} defined at the top of the test-suite.");

            }
        }

        private object EvaluateVariable(ITestVariable variable)
        {
            if (!variable.IsEvaluated())
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                variable.GetValue();
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Time needed for evaluation of variable '{args.VariableName}': {stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")}");
            }

            var output = variable.GetValue();
            return output;
        }
    }
}
