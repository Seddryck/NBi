using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public abstract class BasePredicateFilter : IResultSetFilter
    {
        private ServiceLocator ServiceLocator { get; }
        private Context Context { get; }
        protected readonly IEnumerable<IColumnExpression> expressions;
        protected readonly IEnumerable<IColumnAlias> aliases;

        protected BasePredicateFilter(ServiceLocator serviceLocator, Context context, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            => (ServiceLocator, Context, this.aliases, this.expressions) = (serviceLocator, context, aliases, expressions);

        public ResultSet.ResultSet AntiApply(ResultSet.ResultSet rs)
        {
            return Apply(rs, (x => !x));
        }

        public ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            return Apply(rs, (x => x));
        }

        protected ResultSet.ResultSet Apply(ResultSet.ResultSet rs, Func<bool, bool> onApply)
        {
            var filteredRs = new ResultSet.ResultSet();
            var table = rs.Table.Clone();
            filteredRs.Load(table);
            filteredRs.Table.Clear();

            foreach (DataRow row in rs.Rows)
            {
                Context.Switch(row);
                if (onApply(RowApply(Context)))
                {
                    if (filteredRs.Rows.Count == 0 && filteredRs.Columns.Count != row.Table.Columns.Count)
                    {
                        foreach (DataColumn column in row.Table.Columns)
                        {
                            if (!filteredRs.Columns.Cast<DataColumn>().Any(x => x.ColumnName == column.ColumnName))
                                filteredRs.Columns.Add(column.ColumnName, typeof(object));
                        }
                    }
                    filteredRs.Table.ImportRow(row);
                }
            }

            filteredRs.Table.AcceptChanges();
            return filteredRs;
        }

        protected abstract bool RowApply(Context context);
        public bool Execute(Context context) => RowApply(context);


        protected object GetValueFromRow(Context context, IColumnIdentifier identifier)
        {
            if (identifier is ColumnOrdinalIdentifier)
            {
                var ordinal = (identifier as ColumnOrdinalIdentifier).Ordinal;
                if (ordinal <= context.CurrentRow.Table.Columns.Count)
                    return context.CurrentRow.ItemArray[ordinal];
                else
                    throw new ArgumentException($"The variable of the predicate is identified as '{identifier.Label}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {context.CurrentRow.Table.Columns.Count} columns.");
            }

            var name = (identifier as ColumnNameIdentifier).Name;
            var alias = aliases.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            if (alias != null)
                return context.CurrentRow.ItemArray[alias.Column];

            var expression = expressions.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            if (expression != null)
            {
                var result = EvaluateExpression(expression, context);
                var expColumnName = $"exp::{name}";
                if (!context.CurrentRow.Table.Columns.Contains(expColumnName))
                {
                    var newColumn = new DataColumn(expColumnName, typeof(object));
                    context.CurrentRow.Table.Columns.Add(newColumn);
                }

                context.CurrentRow[expColumnName] = result;
                return result;
            }

            var column = context.CurrentRow.Table.Columns.Cast<DataColumn>().SingleOrDefault(x => string.Equals(x.ColumnName, name, StringComparison.OrdinalIgnoreCase));
            if (column != null)
                return context.CurrentRow[column.ColumnName];

            var existingNames = context.CurrentRow.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName)
                .Union(aliases.Select(x => x.Name)
                .Union(expressions.Select(x => x.Name)));

            throw new ArgumentException($"The value '{name}' is not recognized as a column position, a column name, a column alias or an expression. Possible arguments are: '{string.Join("', '", existingNames.ToArray())}'");
        }

        protected object EvaluateExpression(IColumnExpression expression, Context context)
        {
            if (expression.Language == LanguageType.NCalc)
            {
                var exp = new NCalc.Expression(expression.Value);
                var factory = new ColumnIdentifierFactory();

                exp.EvaluateParameter += delegate (string name, NCalc.ParameterArgs args)
                {
                    args.Result = GetValueFromRow(context, factory.Instantiate(name));
                };

                return exp.Evaluate();
            }
            else if (expression.Language == LanguageType.Native)
            {
                var parse = expression.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                var variable = new ColumnIdentifierFactory().Instantiate(parse.ElementAt(0));
                var value = GetValueFromRow(context, variable);

                foreach (var nativeFunction in parse.Skip(1))
                {
                    var factory = new NativeTransformationFactory(ServiceLocator, context);
                    var transformation = factory.Instantiate(nativeFunction);
                    value = transformation.Evaluate(value);
                }
                
                return value;
            }
            else
                throw new ArgumentOutOfRangeException($"The language {expression.Language} is not supported during the evaluation of an expression.");
        }

        public abstract string Describe();

        private class TransformationInfo : ITransformationInfo
        {
            public ColumnType OriginalType { get; set; }
            public LanguageType Language { get; set; }
            public string Code { get; set; }
        }
    }
}
