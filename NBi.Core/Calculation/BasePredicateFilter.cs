using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
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
        protected readonly IEnumerable<IColumnExpression> expressions;
        protected readonly IEnumerable<IColumnAlias> aliases;

        protected BasePredicateFilter(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
        {
            this.aliases = aliases;
            this.expressions = expressions;
        }

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
                if (onApply(RowApply(row)))
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

        protected abstract bool RowApply(DataRow row);
        public bool Execute(DataRow row) => RowApply(row);


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
            var alias = aliases.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            if (alias != null)
                return row.ItemArray[alias.Column];

            var expression = expressions.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            if (expression != null)
            {
                var result = EvaluateExpression(expression, row);
                var expColumnName = $"exp::{name}";
                if (!row.Table.Columns.Contains(expColumnName))
                {
                    var newColumn = new DataColumn(expColumnName, typeof(object));
                    row.Table.Columns.Add(newColumn);
                }

                row[expColumnName] = result;
                return result;
            }

            var column = row.Table.Columns.Cast<DataColumn>().SingleOrDefault(x => string.Equals(x.ColumnName, name, StringComparison.OrdinalIgnoreCase));
            if (column != null)
                return row[column.ColumnName];

            var existingNames = row.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName)
                .Union(aliases.Select(x => x.Name)
                .Union(expressions.Select(x => x.Name)));

            throw new ArgumentException($"The value '{name}' is not recognized as a column position, a column name, a column alias or an expression. Possible arguments are: '{string.Join("', '", existingNames.ToArray())}'");
        }

        protected object EvaluateExpression(IColumnExpression expression, DataRow row)
        {
            var exp = new NCalc.Expression(expression.Value);
            var factory = new ColumnIdentifierFactory();

            exp.EvaluateParameter += delegate (string name, NCalc.ParameterArgs args)
            {
                args.Result = GetValueFromRow(row, factory.Instantiate(name));
            };

            return exp.Evaluate();
        }

        public abstract string Describe();
    }
}
