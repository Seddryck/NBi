using System.Xml.Serialization;

namespace NBi.Model.Xml.Items.ResultSet;

public class KeyXml
{
    [XmlAttribute("index")]
    public int Index { get; set; }
}
