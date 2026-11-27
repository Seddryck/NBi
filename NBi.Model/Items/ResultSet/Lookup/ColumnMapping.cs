using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.ResultSet.Lookup;

public class ColumnMapping
{
    public string Candidate { get; set; }
    public string Reference { get; set; }
    public ColumnType Type { get; set; }

    public string Tolerance { get; set; }
}
