using NBi.Model.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Calculation.Grouping;

public class GroupBy
{
    public List<ColumnDefinitionLight> Columns { get; set; } = new List<ColumnDefinitionLight>();

    public List<Case> Cases { get; set; }

    public static GroupBy None { get; } = new GroupByNone();

    public class GroupByNone : GroupBy { }
}
