using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Constraints;
using NBi.Xml.TestCases;

namespace NBi.Xml
{
    public class TestXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlAttribute("uid")]
        public string UniqueIdentifier { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlElement("Category")]
        public List<string> Categories;

        [XmlElement(Type = typeof(SyntacticallyCorrectXml), ElementName = "SyntacticallyCorrect"),
        XmlElement(Type = typeof(FasterThanXml), ElementName = "FasterThan"),
        XmlElement(Type = typeof(EqualToXml), ElementName = "EqualTo"),
        XmlElement(Type = typeof(CountXml), ElementName = "Count")
        ]
        public List<AbstractConstraintXml> Constraints;

        [XmlElement(Type = typeof(QueryXml), ElementName = "Query"),
        XmlElement(Type = typeof(MembersXml), ElementName = "Members")
        ]
        public List<AbstractTestCaseXml> TestCases;

        public TestXml()
        {
            Constraints = new List<AbstractConstraintXml>();
            TestCases = new List<AbstractTestCaseXml>();
            Categories = new List<string>();
        }
    }
}