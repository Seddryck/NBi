using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml
{
    [XmlRoot(ElementName = "testSuite", Namespace = "http://NBi/TestSuite")]
    public class TestSuiteXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlElement("test")]
        public List<TestXml> Tests { get; set; }

        [XmlElement("settings")]
        public SettingsXml Settings { get; set; }

        public TestSuiteXml()
        {
            Tests = new List<TestXml>();
        }
    }
}
