using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Items;

namespace NBi.Xml.Systems;

public class DataTypeXml: AbstractSystemUnderTestXml
{
    [XmlElement(Type = typeof(MeasureXml), ElementName = "measure"),
    XmlElement(Type = typeof(PropertyXml), ElementName = "property"),
    XmlElement(Type = typeof(ColumnXml), ElementName = "column"),
    ]
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
