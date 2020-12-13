using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using NBi.Extensibility;

namespace NBi.Core.Calculation.Grouping
{
    sealed class NoneGrouping : IGroupBy
    {
        public IDictionary<KeyCollection, DataTable> Execute(IResultSet resultSet)
        {
            return new Dictionary<KeyCollection, DataTable>()
            {
                { new KeyCollection(Array.Empty<object>()), resultSet.Table }
            };
        }
    }
}
