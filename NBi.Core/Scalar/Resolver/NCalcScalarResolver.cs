using Microsoft.CSharp;
using NBi.Core.ResultSet;
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
                    ? Args.Context.Variables[name.Substring(1, name.Length - 1)].GetValue()
                    : GetValueFromRow(Args.Context.CurrentRow, factory.Instantiate(name));
            };

            var rawValue = exp.Evaluate();

            return (T)Convert.ChangeType(rawValue, typeof(T));
        }

        protected object GetValueFromRow(DataRow row, IColumnIdentifier identifier)
        {
            if (identifier is ColumnOrdinalIdentifier)
            {
                var ordinal = (identifier as ColumnOrdinalIdentifier).Ordinal;
                if (ordinal <= row.Table.Columns.Count)
                    return row.ItemArray[ordinal];
                else
                    throw new ArgumentException($"The variable of the predicate is identified as '{identifier.Label}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {row.Table.Columns.Count} columns.");
            }

            var name = (identifier as ColumnNameIdentifier).Name;

            var column = row.Table.Columns.Cast<DataColumn>().SingleOrDefault(x => string.Equals(x.ColumnName, name, StringComparison.OrdinalIgnoreCase));
            if (column != null)
                return row[column.ColumnName];

            var existingNames = row.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName);
            
            throw new ArgumentException($"The value '{name}' is not recognized as a column position, a column name, a column alias or an expression. Possible arguments are: '{string.Join("', '", existingNames.ToArray())}'");
        }

        object IResolver.Execute() => Execute();
    }
}