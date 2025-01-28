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
//    class AllRowsStrategy : IStrategy
//    {
//        public bool Execute(ResultSet resultSet, IPredicateInfo predicateInfo, Func<DataRow, IColumnIdentifier, object> getValueFromRow)
//        {
//            var result = true;
//            var factory = new PredicateFactory();
//            var InternalPredicate = factory.Instantiate(predicateInfo);

//            var enumeratorRow = resultSet.Rows.GetEnumerator();
//            while (enumeratorRow.MoveNext() && result)
//            {
//                var value = getValueFromRow(enumeratorRow.Current as DataRow, predicateInfo.Operand);
//                result = InternalPredicate.Execute(value);
//            }
//            return result;
//        }
//    }
//}
