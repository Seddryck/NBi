using System;
using System.Collections.Generic;
using NBi.Model.Items;

namespace NBi.Model.Systems;

public class DataType: AbstractSystemUnderTest
{
    public AbstractItem Item { get; set; }

    public override BaseItem BaseItem
    {
        get
        {
            return Item;
        }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        return Item.GetRegexMatch();
    }

    public override ICollection<string> GetAutoCategories()
    {
        var values = Item.GetAutoCategories();
        values.Add("Data-type");
        return values;
    }
}
