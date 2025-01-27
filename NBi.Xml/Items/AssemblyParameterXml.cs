using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using NBi.Core;

namespace NBi.Xml.Items;

public class AssemblyParameterXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlText]
    public string StringValue { get; set; }

    public object Value
    {
        get
        {
            return StringValue;
        }
    }
}
