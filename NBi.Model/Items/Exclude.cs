using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class Exclude
{
    public List<string> Items { get; set; }

    public List<PatternDefinition> Patterns { get; set; }

    public Exclude()
    {
        Items = new List<string>();
    }
}
