using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility;

public interface IResultSet
{
    //Whole result-set
    IResultSet Clone();
    void Clear();

    //Columns
    IEnumerable<IResultColumn> Columns { get; }
    int ColumnCount { get; }
    
    IResultColumn? GetColumn(IColumnIdentifier columnIdentifier);
    IResultColumn? GetColumn(string name);
    IResultColumn? GetColumn(int index);

    IResultColumn AddColumn(string name);
    IResultColumn AddColumn(string name, Type type);
    IResultColumn AddColumn(string name, int ordinal, Type type);

    bool ContainsColumn(string name);

    //Rows 
    IEnumerable<IResultRow> Rows { get; }
    int RowCount { get; }

    IResultRow this[int index] { get; }

    IResultRow NewRow();
    IResultRow AddRow(IResultRow row);
    IResultRow AddRow(object?[] itemArray);
    void AddRange(IEnumerable<IResultRow> rows);
    void AcceptChanges();
}
