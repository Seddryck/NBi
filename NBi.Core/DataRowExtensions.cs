using NBi.Core.ResultSet;
using NBi.Extensibility;
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
        public static object? GetValue(this IResultRow row, IColumnIdentifier columnIdentifier) => columnIdentifier.GetValue(row);
        internal static object? GetValue(this DataRow row, IColumnIdentifier columnIdentifier) => columnIdentifier.GetValue(new DataRowResultSet(row));
    }

    public static class DataTableExtensions
    {
        public static IResultColumn? GetColumn(this IResultSet table, IColumnIdentifier columnIdentifier) => columnIdentifier.GetColumn(table);
    }
}
