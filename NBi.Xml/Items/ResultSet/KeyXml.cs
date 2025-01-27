using System.Xml.Serialization;

namespace NBi.Xml.Items.ResultSet;

public class KeyXml
{
    [XmlAttribute("index")]
    public int Index { get; set; }
}
