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

//namespace NBi.Core.ResultSet.Alteration.Projection
//{
//    class RemoveWhileCondition : HoldWhileCondition
//    {
//        protected readonly IAlteration baseAlteration;

//        public RemoveWhileCondition(IStrategy strategy, IPredicateInfo predicateInfo)
//            : this(strategy, predicateInfo, new List<IColumnAlias>(), new List<IColumnExpression>())
//        { }

//        public RemoveWhileCondition(IStrategy strategy, IPredicateInfo predicateInfo, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
//            : this(strategy, predicateInfo, aliases, expressions, new RemoveIdentification(new[] { new ColumnPositionIdentifier(0)}))
//        { }

//        protected RemoveWhileCondition(IStrategy strategy, IPredicateInfo predicateInfo, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IAlteration alteration)
//            : base(strategy, aliases, expressions)
//        {
//            base.predicateInfo = predicateInfo;
//            this.baseAlteration = alteration;
//        }

//        public override ResultSet Execute(ResultSet resultSet)
//        {
//            var result = true;
//            while (result)
//            {
//                result = strategy.Execute(resultSet, predicateInfo, GetValueFromRow);
//                if (result)
//                    baseAlteration.Execute(resultSet);
//            }
//            return resultSet;
//        }

        
//    }
//}
