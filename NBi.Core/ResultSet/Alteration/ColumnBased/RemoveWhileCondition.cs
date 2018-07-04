using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet.Alteration.ColumnBased.Strategy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.ColumnBased
{
    class RemoveWhileCondition : IAlteration
    {
        protected readonly IPredicateInfo predicateInfo;
        protected readonly IEnumerable<IColumnExpression> expressions;
        protected readonly IEnumerable<IColumnAlias> aliases;
        protected readonly IAlteration baseAlteration;
        protected readonly IStrategy strategy; 

        public RemoveWhileCondition(IStrategy strategy, IPredicateInfo predicateInfo)
            : this(strategy, predicateInfo, new List<IColumnAlias>(), new List<IColumnExpression>())
        { }

        public RemoveWhileCondition(IStrategy strategy, IPredicateInfo predicateInfo, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : this(strategy, predicateInfo, aliases, expressions, new RemoveIdentification(new[] { new ColumnPositionIdentifier(0) }))
        { }

        protected RemoveWhileCondition(IStrategy strategy, IPredicateInfo predicateInfo, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IAlteration alteration)
        {
            this.strategy = strategy;
            this.predicateInfo = predicateInfo;
            this.expressions = expressions;
            this.aliases = aliases;
            this.baseAlteration = alteration;
        }

        public ResultSet Execute(ResultSet resultSet)
        {
            var rs = strategy.Execute(resultSet, predicateInfo, baseAlteration, GetValueFromRow);
            return rs;
        }

        protected object GetValueFromRow(DataRow row, IColumnIdentifier identifier)
        {
            if (identifier is ColumnPositionIdentifier)
            {
                var ordinal = (identifier as ColumnPositionIdentifier).Position;
                if (ordinal <= row.Table.Columns.Count)
                    return row.ItemArray[ordinal];
                else
                    throw new ArgumentException($"The variable of the predicate is identified as '{identifier.Label}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {row.Table.Columns.Count} columns.");
            }

            var name = (identifier as ColumnNameIdentifier).Name;
            var alias = aliases.SingleOrDefault(x => x.Name == name);
            if (alias != null)
                return row.ItemArray[alias.Column];

            var expression = expressions.SingleOrDefault(x => x.Name == name);
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

            var column = row.Table.Columns.Cast<DataColumn>().SingleOrDefault(x => x.ColumnName == name);
            if (column != null)
                return row[column.ColumnName];

            throw new ArgumentException($"The value '{name}' is not recognized as a column name or a column position or a column alias or an expression.");
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
    }
}
