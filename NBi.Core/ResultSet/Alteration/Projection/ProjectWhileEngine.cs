//using NBi.Core.Calculation;
//using NBi.Core.Calculation.InternalPredicate;
//using NBi.Core.Evaluate;
//using NBi.Core.ResultSet.Alteration.ColumnBased.Strategy;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NBi.Core.ResultSet.Alteration.ColumnBased
//{
//    class ProjectWhileEngine : IAlteration
//    {
//        protected IPredicateInfo predicateInfo;
//        protected readonly IEnumerable<IColumnExpression> expressions;
//        protected readonly IEnumerable<IColumnAlias> aliases;
//        protected readonly IStrategy strategy;

//        public HoldWhileCondition(IStrategy strategy, IPredicateInfo predicateInfo)
//            : this(strategy, predicateInfo, new List<IColumnAlias>(), new List<IColumnExpression>())
//        { }

//        protected HoldWhileCondition(IStrategy strategy, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
//        {
//            this.strategy = strategy;
//            this.expressions = expressions;
//            this.aliases = aliases;
//        }

//        public HoldWhileCondition(IStrategy strategy, IPredicateInfo predicateInfo, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
//            : this(strategy, aliases, expressions)
//        {
//            if (!(predicateInfo.Operand is ColumnDynamicIdentifier))
//                throw new ArgumentException("The identification of the operand must be dynamic identification starting by a '&'");

//            this.predicateInfo = predicateInfo;
//        }

//        public virtual ResultSet Execute(ResultSet resultSet)
//        {
//            var identifiers = new List<IColumnIdentifier>();
//            var result = true;
//            int i = 0;
//            while (result && i < resultSet.ColumnCount)
//            {
//                var currentIdentifier = new ColumnPositionIdentifier(i);
//                predicateInfo.Operand = currentIdentifier;
//                result = strategy.Execute(resultSet, predicateInfo, GetValueFromRow);
//                if (result)
//                    identifiers.Add(currentIdentifier);
//                i += 1;
//            }
//            var holdAction = new HoldIdentification(identifiers);
//            holdAction.Execute(resultSet);
//            return resultSet;
//        }

//        protected object GetValueFromRow(DataRow row, IColumnIdentifier identifier)
//        {
//            if (identifier is ColumnPositionIdentifier)
//            {
//                var ordinal = (identifier as ColumnPositionIdentifier).Position;
//                if (ordinal <= row.table.ColumnCount)
//                    return row.ItemArray[ordinal];
//                else
//                    throw new ArgumentException($"The variable of the InternalPredicate is identified as '{identifier.Label}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {row.table.ColumnCount} columns.");
//            }

//            var name = (identifier as ColumnNameIdentifier).Name;
//            var alias = aliases.SingleOrDefault(x => x.Name == name);
//            if (alias != null)
//                return row.ItemArray[alias.Column];

//            var expression = expressions.SingleOrDefault(x => x.Name == name);
//            if (expression != null)
//            {
//                var result = EvaluateExpression(expression, row);
//                var expColumnName = $"exp::{name}";
//                if (!row.Table.ContainsColumn(expColumnName))
//                {
//                    var newColumn = new DataColumn(expColumnName, typeof(object));
//                    row.Table.Columns.Add(newColumn);
//                }

//                row[expColumnName] = result;
//                return result;
//            }

//            var column = row.Table.Columns.Cast<DataColumn>().SingleOrDefault(x => x.Name == name);
//            if (column != null)
//                return row[column.Name];

//            throw new ArgumentException($"The value '{name}' is not recognized as a column name or a column position or a column alias or an expression.");
//        }

//        protected object EvaluateExpression(IColumnExpression expression, DataRow row)
//        {
//            var exp = new NCalc.Expression(expression.Value);
//            var factory = new ColumnIdentifierFactory();

//            exp.EvaluateParameter += delegate (string name, NCalc.ParameterArgs Args)
//            {
//                Args.Result = GetValueFromRow(row, factory.Instantiate(name));
//            };

//            return exp.Evaluate();
//        }
//    }
//}
