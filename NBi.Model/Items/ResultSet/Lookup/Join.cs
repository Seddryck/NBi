using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.ResultSet.Lookup;

public class Join
{
    public List<ColumnMapping> Mappings { get; set; }

    public List<ColumnUsing> Usings { get; set; }

    public Join()
    {
        Mappings = new List<ColumnMapping>();
        Usings = new List<ColumnUsing>();
    }
}
