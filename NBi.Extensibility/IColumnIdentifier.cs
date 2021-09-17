using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility
{
    public interface IColumnIdentifier
    {
        string Label { get; }
        DataColumn GetColumn(IResultSet rs);
        DataColumn GetColumn(DataTable dataTable);
        object GetValue(DataRow dataRow);
    }
}
