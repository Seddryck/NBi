using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.Model.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Extension;

public class Extend : AlterationDefinition
{
    public string IdentifierSerializer { get; set; }
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }

    public Script Script { get; set; }
}
