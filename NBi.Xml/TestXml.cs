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

        [XmlElement("category")]
        public List<string> Categories;

        [XmlArray("assert"),
        XmlArrayItem(Type = typeof(SyntacticallyCorrectXml), ElementName = "syntacticallyCorrect"),
        XmlArrayItem(Type = typeof(FasterThanXml), ElementName = "fasterThan"),
        XmlArrayItem(Type = typeof(EqualToXml), ElementName = "equalTo"),
        XmlArrayItem(Type = typeof(CountXml), ElementName = "count"),
        XmlArrayItem(Type = typeof(ContainsXml), ElementName = "contains"),
        ]
        public List<AbstractConstraintXml> Constraints;

        [XmlArray("system-under-test"),
        XmlArrayItem(Type = typeof(QueryXml), ElementName = "query"),
        XmlArrayItem(Type = typeof(MembersXml), ElementName = "members")
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