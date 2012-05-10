using System.Xml.Serialization;

namespace NBi.Xml.TestCases
{
    public abstract class AbstractTestCaseXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public abstract object Instantiate();
    }
}
