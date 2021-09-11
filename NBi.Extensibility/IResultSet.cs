using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility
{
    public interface IResultSet
    {
        DataColumnCollection Columns { get; }
        DataRowCollection Rows { get; }

        DataColumn GetColumn(IColumnIdentifier columnIdentifier);

        void Add(DataRow row);
        void ImportRow(DataRow row);
        DataRow NewRow();
        void AddRange(IEnumerable<DataRow> rows);
        void AcceptChanges();

        IResultSet Clone();
        void Clear();
    }
}
