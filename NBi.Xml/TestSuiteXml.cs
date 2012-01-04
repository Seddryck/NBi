using System.Collections.Generic;
using System.Xml.Serialization;

namespace NBi.Xml
{
    [XmlRoot("TestSuite")]
    public class TestSuiteXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlElement("Test")]
        public List<TestXml> Tests { get; set; }
    }
}
