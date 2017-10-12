using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public class PredicateFilter : IResultSetFilter
    {
        private readonly IEnumerable<IColumnExpression> expressions;
        private readonly IEnumerable<IColumnAlias> aliases;
        private readonly IPredicateInfo predicateInfo;

        public PredicateFilter(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IPredicateInfo predicateInfo)
        {
            this.aliases = aliases;
            this.expressions = expressions;
            this.predicateInfo = predicateInfo;
        }

        public ResultSet.ResultSet AntiApply(ResultSet.ResultSet rs)
        {
            return Apply(rs, (x => !x));
        }

        public ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            return Apply(rs, (x => x));
        }

        protected ResultSet.ResultSet Apply(ResultSet.ResultSet rs, Func<bool,bool> onApply)
        {
            var filteredRs = new ResultSet.ResultSet();
            var table = rs.Table.Clone();
            filteredRs.Load(table);
            

            var factory = new PredicateFactory();
            var predicate = factory.Get(predicateInfo);

            foreach (DataRow row in rs.Rows)
            {
                var value = GetValueFromRow(row, predicateInfo.Name);
                if (onApply(predicate.Apply(value)))
                    filteredRs.Table.ImportRow(row);
            }

            filteredRs.Table.AcceptChanges();
            return filteredRs;
        }

        protected object GetValueFromRow(DataRow row, string name)
        {
            if (name.StartsWith("[") && name.EndsWith("]"))
                name = name.Substring(1, name.Length - 2);

            if (name.StartsWith("#"))
            {
                if (int.TryParse(name.Replace("#", ""), out var ordinal))
                    if (ordinal <= row.Table.Columns.Count)
                        return row.ItemArray[ordinal];
                    else
                        throw new ArgumentException($"The variable of the predicate is identified as '{name}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {row.Table.Columns.Count} columns.");
                else
                    throw new ArgumentException($"The variable of the predicate is identified as '{name}'. All names starting by a '#' matches to a column position and must be followed by an integer.");
            }

            var alias = aliases.SingleOrDefault(x => x.Name == name);
            if (alias != null)
                return row.ItemArray[alias.Column];

            var expression = expressions.SingleOrDefault(x => x.Name == name);
            if (expression != null)
                return EvaluateExpression(expression, row);

            var column = row.Table.Columns.Cast<DataColumn>().SingleOrDefault(x => x.ColumnName == name);
            if (column != null)
                return row[column.ColumnName];

            throw new ArgumentException($"The value '{name}' is not recognized as a column name or a column position or a column alias or an expression.");
        }

        private object EvaluateExpression(IColumnExpression expression, DataRow row)
        {
            var exp = new NCalc.Expression(expression.Value);

            exp.EvaluateParameter += delegate (string name, NCalc.ParameterArgs args)
            {
                args.Result=GetValueFromRow(row, name);
            };

            return exp.Evaluate();
        }
    }
}
