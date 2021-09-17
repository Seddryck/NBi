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
        IEnumerable<IResultRow> Rows { get; }
        int RowCount { get; }
        DataColumn GetColumn(IColumnIdentifier columnIdentifier);

        IResultRow NewRow();
        IResultRow Add(IResultRow row);
        void AddRange(IEnumerable<IResultRow> rows);
        IResultRow this[int index] { get; }
        
        
        void AcceptChanges();

        void InsertAt(IResultRow row, int index);
        void RemoveAt(int index);

        IResultSet Clone();
        void Clear();
    }

    public interface IResultRow
    {
        object this[int index] { get; set; }
        object this[string columnName] { get; set; }
        object this[IColumnIdentifier identifier] { get; }
        object[] ItemArray { get; set; }

        T Field<T>(int ordinal);
        bool IsNull(int index);
        bool IsNull(string columnName);
        IResultSet Parent { get; }
        void SetColumnError(string columnName, string message);
        void SetColumnError(int index, string message);
        string GetColumnError(int index);
        string GetColumnError(string columnName);
        void Delete();
    }
}
