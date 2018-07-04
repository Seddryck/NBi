using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.ColumnBased.Strategy
{
    class AllRowsStrategy : IStrategy
    {
        public bool Execute(ResultSet resultSet, IPredicateInfo predicateInfo, Func<DataRow, IColumnIdentifier, object> getValueFromRow)
        {
            var result = true;
            var factory = new PredicateFactory();
            var predicate = factory.Instantiate(predicateInfo);

            var enumeratorRow = resultSet.Rows.GetEnumerator();
            while (enumeratorRow.MoveNext() && result)
            {
                var value = getValueFromRow(enumeratorRow.Current as DataRow, predicateInfo.Operand);
                result = predicate.Execute(value);
            }
            return result;
        }
    }
}
