using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Hierarchical.Json;

public class JsonPathXml
{
    [XmlElement("from")]
    public JsonFromXml From { get; set; }
    [XmlElement("select")]
    public List<JsonSelectXml> Selects { get; set; } = new List<JsonSelectXml>();
}
