using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model;

public class Variation
{
    public string Name { get; set; }

    public ColumnType Type { get; set; }

    public List<string> Values { get; set; }
}
