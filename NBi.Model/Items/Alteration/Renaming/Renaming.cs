using NBi.Core.ResultSet;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Renaming;

public class Renaming : AlterationDefinition
{
    public string IdentifierSerializer { get; set; }
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }

    public string NewName { get; set; }

    public MissingColumn Missing { get; set; } = new MissingColumn { Behavior = MissingColumnBehavior.Failure };

    public bool MissingSpecified
    {
        get => Missing.Behavior != MissingColumnBehavior.Failure;
        set { }
    }
}
