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
    class TopRowsStrategy : IStrategy
    {
        public int Value { get => 1; }

        public bool Execute(ResultSet resultSet, IPredicateInfo predicateInfo, Func<DataRow, IColumnIdentifier, object> getValueFromRow)
        {
            var result = false;
            var factory = new PredicateFactory();
            var predicate = factory.Instantiate(predicateInfo);
            var i = 0;
            var enumeratorRow = resultSet.Rows.GetEnumerator();

            while (enumeratorRow.MoveNext() && !result && i < Value)
            {
                var value = getValueFromRow(enumeratorRow.Current as DataRow, predicateInfo.Operand);
                result = predicate.Execute(value);
                i++;
            }
            return result;
        }
    }
}
