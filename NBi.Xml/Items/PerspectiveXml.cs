using NBi.Xml.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items;


public class PerspectiveXml : AbstractItem, IOwnerFilter
{
    [XmlAttribute("owner")]
    public string Owner { get; set; }

    [XmlIgnore]
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
