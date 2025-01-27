using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Query;
using NBi.Extensibility.Query;

namespace NBi.Xml.Items;

public class QueryTemplateVariableXml : IQueryTemplateVariable
{
    [XmlAttribute("name")]
    public string Name { get; set; }
    
    [XmlText]
    public string Value { get; set; }
}
