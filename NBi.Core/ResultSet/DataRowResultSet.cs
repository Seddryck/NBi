using NBi.Extensibility;
using Expressif.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet;

class DataRowResultSet(DataRow dataRow) : IResultRow
{
    private DataRow DataRow { get; } = dataRow;
    private IResultSet? ResultSet { get; set; }

    public object? this[int index] 
    { 
        get => DataRow[index]; 
        set => DataRow[index] = value; 
    }

    public object? this[string columnName] { get => DataRow[columnName]; set => DataRow[columnName] = value; }
    public object? this[IColumnIdentifier identifier] { get => DataRow.GetValue(identifier); }

    public object?[] ItemArray { get => DataRow.ItemArray; set => DataRow.ItemArray = value; }
    public T? Field<T>(int ordinal) => DataRow.Field<T>(ordinal); 
    public bool IsNull(int index) => DataRow.IsNull(index);
    public bool IsNull(string columnName) => DataRow.IsNull(columnName);
    public IResultSet Parent { get => ResultSet ??= new DataTableResultSet(DataRow.Table); }

    public int ColumnCount => Parent.ColumnCount;

    public void SetColumnError(string columnName, string message)
        => DataRow.SetColumnError(columnName, message);
    public void SetColumnError(int index, string message)
        => DataRow.SetColumnError(index, message);

    public string GetColumnError(string columnName)
        => DataRow.GetColumnError(columnName);
    public string GetColumnError(int index)
        => DataRow.GetColumnError(index);

    public void Delete()
        => DataRow.Delete();
    public bool ContainsColumn(string columnName) => Parent.ContainsColumn(columnName);
}
