using System.Collections.Generic;
using System.Xml.Serialization;

namespace NBi.Xml
{
    [XmlRoot(ElementName = "testSuite", Namespace = "http://NBi/TestSuite")]
    public class TestSuiteXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlElement("test")]
        public List<TestXml> Tests { get; set; }

        public TestSuiteXml()
        {
            Tests = new List<TestXml>();
        }
    }
}
