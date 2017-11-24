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

        public T Execute()
        {
            if (!args.GlobalVariables.ContainsKey(args.VariableName))
                throw new NBiException($"The variable named '{args.VariableName}' is not defined.");

            var variable = args.GlobalVariables[args.VariableName];

            if (!variable.IsEvaluated())
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                variable.GetValue();
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed for evaluation of variable '{args.VariableName}': {stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")}");
            }

            var output = variable.GetValue();

            IFormatProvider formatProvider = System.Globalization.NumberFormatInfo.InvariantInfo;
            if (typeof(T) == typeof(DateTime))
                formatProvider = System.Globalization.DateTimeFormatInfo.InvariantInfo;
            else if(output.ToString().EndsWith("%"))
                output = output.ToString().Substring(0, output.ToString().Length-1);

            output = Convert.ChangeType(output, typeof(T), formatProvider);
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Variable '{args.VariableName}' evaluated to: {output}");

            return (T)output;
        }
    }
}
