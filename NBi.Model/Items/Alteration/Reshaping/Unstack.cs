using NBi.Core.ResultSet;
using NBi.Model.Items.Calculation.Grouping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Reshaping;

public class Unstack : AlterationDefinition
{
    public Header Header { get; set; }

    public GroupBy GroupBy { get; set; }

    public Unstack()
        => GroupBy = GroupBy.None;

    public bool GroupBySerialized
    {
        get => GroupBy != GroupBy.None;
        set { throw new NotImplementedException(); }
    }

}
