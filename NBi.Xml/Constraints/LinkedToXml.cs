using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Items;

namespace NBi.Xml.Constraints;

public class LinkedToXml : AbstractConstraintXml
{
    [XmlElement(Type = typeof(MeasureGroupXml), ElementName = "measure-group"),
    XmlElement(Type = typeof(DimensionXml), ElementName = "dimension")
    ]
    public AbstractItem Item { get; set; }
}
