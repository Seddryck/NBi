using NBi.Model.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class Perspectives : AbstractItem, IOwnerFilter
{
    public string Owner { get; set; }

    public override string TypeName
    {
        get { return "perspectives"; }
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        values.Add("Perspectives");
        return values;
    }
}
