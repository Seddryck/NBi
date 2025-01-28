using NBi.Xml.Items.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items;


public class PerspectivesXml : AbstractItem, IOwnerFilter
{
    [XmlAttribute("owner")]
    public string Owner { get; set; }

    [XmlIgnore]
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
