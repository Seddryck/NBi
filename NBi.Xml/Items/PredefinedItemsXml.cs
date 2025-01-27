using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Members.Predefined;

namespace NBi.Xml.Items;

public class PredefinedItemsXml
{
    [XmlAttribute("type")]
    public PredefinedMembers Type { get; set; }

    [XmlAttribute("language")]
    public string Language { get; set; }

    public IEnumerable<string> GetItems()
    {
        var factory = new PredefinedMembersFactory();
        return factory.Instantiate(Type, Language);
    }
}
