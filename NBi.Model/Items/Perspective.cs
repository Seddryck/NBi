using NBi.Model.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;


public class Perspective : AbstractItem, IOwnerFilter
{
    public string Owner { get; set; }

    public override string TypeName
    {
        get { return "perspective"; }
    }

    internal override ICollection<string> GetAutoCategories()
    {
        var values = new List<string>();
        values.Add(string.Format("Perspective '{0}'", Caption));
        values.Add("Perspectives");
        return values;
    }
}
