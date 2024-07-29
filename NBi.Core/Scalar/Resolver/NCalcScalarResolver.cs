using Expressif.Values;
using Microsoft.CSharp;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    class NCalcScalarResolver<T> : IScalarResolver<T>
    {
        private NCalcScalarResolverArgs Args { get; }

        public NCalcScalarResolver(NCalcScalarResolverArgs args)
            => Args = args;

        public T Execute()
        {
            var exp = new NCalc.Expression(Args.Code);
            var factory = new ColumnIdentifierFactory();

            exp.EvaluateParameter += delegate (string name, NCalc.ParameterArgs args)
            {
                args.Result = name.StartsWith("@")
                    ? Args.Context.Variables[name]
                    : GetValueFromRow(Args.Context.CurrentRow!, factory.Instantiate(name));
            };

            var rawValue = exp.Evaluate();

            return (T)Convert.ChangeType(rawValue, typeof(T));
        }

        protected virtual object? GetValueFromRow(IResultRow row, IColumnIdentifier identifier)
        {
            if (identifier is ColumnOrdinalIdentifier ordinalIdentifier)
            {
                var ordinal = ordinalIdentifier.Ordinal;
                if (ordinal <= row.Parent.Columns.Count())
                    return row[ordinal];
                else
                    throw new ArgumentException($"The variable of the predicate is identified as '{identifier.Label}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {row.Parent.Columns.Count()} columns.");
            }
            else if (identifier is ColumnNameIdentifier nameIdentifier)
            {
                var name = nameIdentifier.Name;
                if (row.Parent.ContainsColumn(name))
                    return row[name];

                var existingNames = row.Parent.Columns.Select(x => x.Name);
                throw new ArgumentException($"The value '{name}' is not recognized as a column position, a column name, a column alias or an expression. Possible arguments are: '{string.Join("', '", existingNames.ToArray())}'");
            }
            else
                throw new NotImplementedException();
        }

        object? IResolver.Execute() => Execute();
    }
}