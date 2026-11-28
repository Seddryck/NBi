using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Duplication;
using NBi.Extensibility;
using NBi.Model.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Duplication;

public class Output
{
    public string IdentifierSerializer { get; set; }
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }

    public OutputClass Class { get; set; }

    public Script Script { get; set; }

    public string Value { get; set; }
}
