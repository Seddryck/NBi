using NBi.Model.Items.ResultSet;
using NBi.Model.Items.ResultSet.Lookup;
using NBi.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Lookup;

public class LookupReplace : AlterationDefinition
{
    public Missing Missing { get; set; }

    public bool MissingSpecified
    {
        get => Missing.Behavior != Behavior.Failure;
        set { }
    }

    public Join Join { get; set; }

    public ResultSetSystem ResultSet { get; set; }

    public ColumnDefinitionLight Replacement { get; set; }

    public LookupReplace()
        => Missing = new Missing() { Behavior = Behavior.Failure };
}
