using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml
{
    [XmlRoot(ElementName = "test", Namespace = "")]
    public class TestStandaloneXml : TestXml
    {
    }
}
