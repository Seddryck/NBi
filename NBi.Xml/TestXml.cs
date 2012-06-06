using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

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

        [XmlArray("system-under-test"),
        XmlArrayItem(Type = typeof(QueryXml), ElementName = "query"),
        XmlArrayItem(Type = typeof(MembersXml), ElementName = "members"),
        XmlArrayItem(Type = typeof(StructureXml), ElementName = "structure")
        ]
        public List<AbstractSystemUnderTestXml> Systems;

        [XmlArray("assert"),
        XmlArrayItem(Type = typeof(SyntacticallyCorrectXml), ElementName = "syntacticallyCorrect"),
        XmlArrayItem(Type = typeof(FasterThanXml), ElementName = "fasterThan"),
        XmlArrayItem(Type = typeof(EqualToXml), ElementName = "equalTo"),
        XmlArrayItem(Type = typeof(CountXml), ElementName = "count"),
        XmlArrayItem(Type = typeof(ContainsXml), ElementName = "contains"),
        XmlArrayItem(Type = typeof(OrderedXml), ElementName = "ordered"),
        ]
        public List<AbstractConstraintXml> Constraints;

        public TestXml()
        {
            Constraints = new List<AbstractConstraintXml>();
            Systems = new List<AbstractSystemUnderTestXml>();
            Categories = new List<string>();
        }
    }
}