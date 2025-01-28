using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Hierarchical.Json;

public class JsonSelectXml
{
    [XmlText]
    public string Value { get; set; }
}
