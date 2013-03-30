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
            Settings = new SettingsXml();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
                return base.ToString();
            else
                return Name.ToString();
        }

        public void Load(IEnumerable<TestXml> tests)
        {
            foreach (var test in tests)
            {
                if (test is TestStandaloneXml)
                {
                    var t = new TestXml((TestStandaloneXml)test);
                    this.Tests.Add(t);
                }
                else
                    this.Tests.Add(test);
            }
        }
    }
}
