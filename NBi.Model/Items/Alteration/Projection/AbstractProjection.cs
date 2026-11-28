using NBi.Core.ResultSet;
using NBi.Model.Items.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Projection;

public abstract class AbstractProjection : AlterationDefinition
{
    public List<ColumnDefinitionLight> Columns { get; set; } = new List<ColumnDefinitionLight>();
}
