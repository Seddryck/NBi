using Expressif.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility;

public interface IResultRow : ILiteDataRow
{
    new object? this[int index] { get; set; }
    new object? this[string columnName] { get; set; }
    object? this[IColumnIdentifier identifier] { get; }
    object?[] ItemArray { get; set; }
    T? Field<T>(int ordinal);
    bool IsNull(int index);
    bool IsNull(string columnName);
    IResultSet Parent { get; }
    void SetColumnError(string columnName, string message);
    void SetColumnError(int index, string message);
    string GetColumnError(int index);
    string GetColumnError(string columnName);
    void Delete();
}
