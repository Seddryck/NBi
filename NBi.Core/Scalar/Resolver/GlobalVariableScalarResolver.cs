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
            if (!args.GlobalVariables.ContainsKey(args.VariableName))
            {
                var caseIssues = args.GlobalVariables.Keys.Where(k => String.Equals(k, args.VariableName, StringComparison.OrdinalIgnoreCase));

                if (caseIssues.Count() > 0)
                    throw new NBiException($"The variable named '{args.VariableName}' is not defined. Pay attention, variables are case-sensitive. Did you mean '{string.Join("' or '", caseIssues)}'?");

                var arobaseIssues = args.GlobalVariables.Keys.Where(k => String.Equals(k.Replace("@", string.Empty), args.VariableName, StringComparison.OrdinalIgnoreCase));
                if (arobaseIssues.Count() > 0)
                    throw new NBiException($"The variable named '{args.VariableName}' is not defined. Pay attention, variables shouldn't begin with an arobase (@). Consider to review the name of the following variable{(arobaseIssues.Count() == 1 ? string.Empty : "s")}: '{(string.Join("' and '", arobaseIssues))}' at the top of your test-suite.");

                var countMsg =
                    args.GlobalVariables.Count() == 0 ? "No variables are" :
                    args.GlobalVariables.Count() == 1 ? $"1 variable '{(args.GlobalVariables).Keys.ElementAt(0)}' is"
                    : $"{args.GlobalVariables.Count()} other variables are";

                throw new NBiException($"The variable named '{args.VariableName}' is not defined. {countMsg} defined at the top of the test-suite.");

            }


            var variable = args.GlobalVariables[args.VariableName];

            if (!variable.IsEvaluated())
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                variable.GetValue();
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Time needed for evaluation of variable '{args.VariableName}': {stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")}");
            }

            var output = variable.GetValue();

            IFormatProvider formatProvider = System.Globalization.NumberFormatInfo.InvariantInfo;
            if (typeof(T) == typeof(DateTime))
                formatProvider = System.Globalization.DateTimeFormatInfo.InvariantInfo;
            else if (output != null && output.ToString().EndsWith("%"))
                output = output.ToString().Substring(0, output.ToString().Length - 1);

            output = Convert.ChangeType(output, typeof(T), formatProvider);
            var invariantCulture = new CultureFactory().Invariant;
            var msg = $@"Variable '{args.VariableName}' evaluated to: {
                    (
                        output == null ? "(null)" :
                        output is string && string.IsNullOrEmpty(output.ToString()) ? "(empty)" : output
                    )
                }
            ";
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, msg.ToString(invariantCulture));

            return (T)output;
        }

        object IScalarResolver.Execute() => Execute();
    }
}
