using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility;

public interface IColumnIdentifier
{
    string Label { get; }
    IResultColumn? GetColumn(IResultSet rs);
    object? GetValue(IResultRow dataRow);
}
