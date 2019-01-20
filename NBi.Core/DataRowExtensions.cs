using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core
{
    public static class DataRowExtensions
    {
        public static object GetValue(this DataRow row, IColumnIdentifier columnIdentifier) => columnIdentifier.GetValue(row);
    }

    public static class DataTableExtensions
    {
        public static DataColumn GetColumn(this DataTable table, IColumnIdentifier columnIdentifier) => columnIdentifier.GetColumn(table);
    }
}
