using NBi.Core.Calculation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.ColumnBased.Strategy
{
    public interface IStrategy
    {
        bool Execute(ResultSet resultSet, IPredicateInfo predicateInfo, Func<DataRow, IColumnIdentifier, object> getValueFromRow);
    }
}
