//using NBi.Core.Calculation;
//using NBi.Core.Calculation.InternalPredicate;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NBi.Core.ResultSet.Alteration.Projection.Strategy
//{
//    class TopRowsStrategy : IStrategy
//    {
//        public int Value { get => 1; }

//        public bool Execute(ResultSet resultSet, IPredicateInfo predicateInfo, Func<DataRow, IColumnIdentifier, object> getValueFromRow)
//        {
//            var result = false;
//            var factory = new PredicateFactory();
//            var InternalPredicate = factory.Instantiate(predicateInfo);
//            var i = 0;
//            var enumeratorRow = resultSet.Rows.GetEnumerator();

//            while (enumeratorRow.MoveNext() && !result && i < Value)
//            {
//                var value = getValueFromRow(enumeratorRow.Current as DataRow, predicateInfo.Operand);
//                result = InternalPredicate.Execute(value);
//                i++;
//            }
//            return result;
//        }
//    }
//}
