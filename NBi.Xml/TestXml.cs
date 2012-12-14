using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        [XmlAttribute("ignore")]
        public bool Ignore { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlElement("category")]
        public List<string> Categories;

        [XmlArray("system-under-test"),
        XmlArrayItem(Type = typeof(QueryXml), ElementName = "query"),
        XmlArrayItem(Type = typeof(MembersXml), ElementName = "members"),
        XmlArrayItem(Type = typeof(StructureXml), ElementName = "structure"),
        ]
        public List<AbstractSystemUnderTestXml> Systems;

        [XmlArray("assert"),
        XmlArrayItem(Type = typeof(SyntacticallyCorrectXml), ElementName = "syntacticallyCorrect"),
        XmlArrayItem(Type = typeof(FasterThanXml), ElementName = "fasterThan"),
        XmlArrayItem(Type = typeof(EqualToXml), ElementName = "equalTo"),
        XmlArrayItem(Type = typeof(CountXml), ElementName = "count"),
        XmlArrayItem(Type = typeof(ContainsXml), ElementName = "contains"),
        XmlArrayItem(Type = typeof(ExistsXml), ElementName = "exists"),
        XmlArrayItem(Type = typeof(OrderedXml), ElementName = "ordered"),
        ]
        public List<AbstractConstraintXml> Constraints;

        public TestXml()
        {
            Constraints = new List<AbstractConstraintXml>();
            Systems = new List<AbstractSystemUnderTestXml>();
            Categories = new List<string>();
        }

        public string GetName()
        {
            string newName = Name;
            if (Systems[0] != null)
            {
                var vals = Systems[0].GetRegexMatch();

                Regex re = new Regex(@"\{(sut:([a-z\-])*?)\}", RegexOptions.Compiled);
                string key = string.Empty;
                try
                {

                    newName = re.Replace(Name, delegate(Match match)
                                {
                                    key = match.Groups[1].Value;
                                    return vals[key];
                                });
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine(string.Format("Unknown tag '{0}' in test name has stopped the replacement of tag in test name", key));
                }
            }
            return newName;
        }

        [XmlIgnore()]
        public string Content { get; set; }
    }
}