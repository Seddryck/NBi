using NBi.Core.ResultSet.Alteration.Merging;
using NBi.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Merging;

public class Union : Merge
{
    public ColumnIdentity ColumnIdentity { get; set; } = ColumnIdentity.Ordinal;
}
