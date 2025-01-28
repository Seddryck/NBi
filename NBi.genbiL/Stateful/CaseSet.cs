using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful;

public class CaseSet
{
    public DataTable Content { get; set; }
    public IEnumerable<string> Variables
       => Content.Columns.Cast<DataColumn>().Select(x => x.ColumnName);

    public CaseSet() => Content = new DataTable();
}
