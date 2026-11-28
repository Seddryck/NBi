using NBi.Model.Systems;
using NBi.Model.Variables.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Merging;

public class Merge : AlterationDefinition
{
    public ResultSetSystem ResultSet { get; set; }
}
