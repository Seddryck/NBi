//using NBi.Core.Calculation.Predicate;
//using NBi.Core.Evaluate;
//using NBi.Core.Injection;
//using NBi.Core.ResultSet;
//using NBi.Core.Variable;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NBi.Core.Calculation
//{
//    class SinglePredicateFilter : RowValueExtractor
//    {
//        private readonly Func<object, bool> implementation;
//        private readonly IColumnIdentifier operand;
//        private readonly Func<string> describeFunction;

//        public SinglePredicateFilter(ServiceLocator serviceLocator, Context context, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IColumnIdentifier operand, Func<object, bool> implementation, Func<string> describeFunction)
//            : base(serviceLocator, context, aliases, expressions)
//        {
//            this.operand = operand;
//            this.implementation = implementation;
//            this.describeFunction = describeFunction;
//        }
        
//        protected override bool RowApply(Context context)
//        {
//            var value = GetValueFromRow(context, operand);
//            return implementation(value);
//        }

//        public override string Describe()
//        {
//            return $"{operand.Label} {describeFunction()}.";
//        }
//    }
//}
