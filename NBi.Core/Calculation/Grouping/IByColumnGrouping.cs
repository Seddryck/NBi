using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;

namespace NBi.Core.Calculation.Grouping
{
    public interface IByColumnGrouping
    {
        IDictionary<KeyCollection, DataTable> Execute(ResultSet.ResultSet resultSet);
    }
}
