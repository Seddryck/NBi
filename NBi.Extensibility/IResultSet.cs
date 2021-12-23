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
        IEnumerable<IResultColumn> Columns { get; }
        IEnumerable<IResultRow> Rows { get; }
        int RowCount { get; }
        IResultColumn GetColumn(IColumnIdentifier columnIdentifier);

        IResultRow NewRow();
        IResultRow Add(IResultRow row);
        void AddRange(IEnumerable<IResultRow> rows);
        IResultRow this[int index] { get; }
        
        void AcceptChanges();

        void InsertAt(IResultRow row, int index);
        void RemoveAt(int index);

        IResultSet Clone();
        void Clear();

        IResultColumn AddColumn(string name);
        IResultColumn AddColumn(string name, Type type);
        IResultColumn AddColumn(string name, int ordinal, Type type);

        bool ContainsColumn(string name);
        IResultColumn GetColumn(string name);
        IResultColumn GetColumn(int index);
        int ColumnCount { get; }
    }
}
