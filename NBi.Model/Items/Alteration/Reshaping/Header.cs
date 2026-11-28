using NBi.Model.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Reshaping;

public class Header
{
    public ColumnDefinitionLight Column { get; set; }

    public List<string> EnforcedValues { get; set; } = new List<string>();
}
