using Expressif.Values;
using NBi.Core.Variable;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
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
        private GlobalVariableScalarResolverArgs Args { get; }
        private static readonly object locker = new();

        public GlobalVariableScalarResolver(GlobalVariableScalarResolverArgs args)
            => Args = args;

        public GlobalVariableScalarResolver(string name, Context context)
            : this(new GlobalVariableScalarResolverArgs(name, context)) { }

        public T? Execute()
        {
            CheckVariableExists(Args.VariableName, Args.Context.Variables);
            var evaluation = EvaluateVariable(Args.Context.Variables[Args.VariableName]!);
            var typedEvaluation = StrongTypingVariable(evaluation);
            DisplayVariable(Args.VariableName, typedEvaluation);

            return (T?)typedEvaluation;
        }

        object? IResolver.Execute() => Execute();

        protected virtual void DisplayVariable(string name, object? value)
        {
            var invariantCulture = new CultureFactory().Invariant;
            var msg = $@"Variable '{name}' used with value: {(
                        value == null ? "(null)" :
                        value is string && string.IsNullOrEmpty(value.ToString()) ? "(empty)" :
                        value
                    )}
            ";
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, msg.ToString(invariantCulture));
        }

        private static object? StrongTypingVariable(object? input)
        {
            IFormatProvider formatProvider = typeof(T) == typeof(DateTime)
                ? System.Globalization.DateTimeFormatInfo.InvariantInfo
                : System.Globalization.NumberFormatInfo.InvariantInfo;

            if (input != null && input.ToString()!.EndsWith("%"))
                input = input.ToString()![..^1];

            var output = Convert.ChangeType(input, typeof(T), formatProvider);
            return output;
        }

        protected virtual void CheckVariableExists(string name, ContextVariables variables)
        {
            if (!variables.Contains(name))
            {
                var caseIssues = variables.Keys.Where(k => String.Equals(k, name, StringComparison.OrdinalIgnoreCase));

                if (caseIssues.Any())
                    throw new NBiException($"The variable named '{name}' is not defined. Pay attention, variables are case-sensitive. Did you mean '{string.Join("' or '", caseIssues)}'?");

                var arobaseIssues = variables.Keys.Where(k => String.Equals(k.Replace("@", string.Empty), name, StringComparison.OrdinalIgnoreCase));
                if (arobaseIssues.Any())
                    throw new NBiException($"The variable named '{name}' is not defined. Pay attention, variables shouldn't begin with an arobase (@). Consider to review the name of the following variable{(arobaseIssues.Count() == 1 ? string.Empty : "s")}: '{(string.Join("' and '", arobaseIssues))}' at the top of your test-suite.");

                var countMsg =
                    variables.Count == 0 ? "No variables are" :
                    variables.Count == 1 ? $"1 variable '{(variables).Keys.ElementAt(0)}' is"
                    : $"{variables.Count} other variables are";

                throw new NBiException($"The variable named '{name}' is not defined. {countMsg} defined at the top of the test-suite.");
            }
        }

        protected virtual object? EvaluateVariable(object value)
        {
            if (value is IVariable variable)
            {
                lock (locker)
                {
                    if (variable is RuntimeVariable runtime && !runtime.IsEvaluated())
                        runtime.Evaluate();
                }
                return variable.GetValue();
            }
            return value;
        }
    }
}
