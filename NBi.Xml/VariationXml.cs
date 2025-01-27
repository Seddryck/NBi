using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml;

public class VariationXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("type")]
    [DefaultValue(ColumnType.Text)]
    public ColumnType Type { get; set; }

    [XmlElement("value")]
    public List<string> Values { get; set; }
}
