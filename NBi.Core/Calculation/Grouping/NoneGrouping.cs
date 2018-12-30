using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;

namespace NBi.Core.Calculation.Grouping
{
    sealed class NoneGrouping : IByColumnGrouping
    {
        public IDictionary<KeyCollection, DataTable> Execute(ResultSet.ResultSet resultSet)
        {
            return new Dictionary<KeyCollection, DataTable>()
            {
                { new KeyCollection(new object[]{ }), resultSet.Table }
            };
        }
    }
}
