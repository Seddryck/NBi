using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.ResultSet.Lookup;

public class ColumnUsing
{
    public string Column { get; set; }

    public ColumnType Type { get; set; }

    public string Tolerance { get; set; }
}
