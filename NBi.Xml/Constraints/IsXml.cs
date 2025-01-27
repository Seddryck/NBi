using System.ComponentModel;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints;

public class IsXml : AbstractConstraintXml
{
    private string value;
    [XmlText]
    public string Value 
    {
        get { return value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", ""); } 
        set { this.value=value; } 
    }
}
