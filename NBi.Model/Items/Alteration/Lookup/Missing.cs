using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Lookup;

public class Missing
{
    public Behavior Behavior { get; set; } = Behavior.Failure;

    public string DefaultValue { get; set; }
}

public enum Behavior
{
    Failure = 0,
    OriginalValue = 1,
    DefaultValue = 2,
    DiscardRow = 3
}
