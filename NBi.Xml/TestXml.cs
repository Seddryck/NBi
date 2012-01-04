using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework.Constraints;

namespace NBi.Xml
{
    public class TestXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlAttribute("uid")]
        public string UniqueIdentifier { get; set; }

        [XmlElement(Type = typeof(QueryParserXml), ElementName="QueryParser"),
        XmlElement(Type = typeof(QueryPerformanceXml), ElementName = "QueryPerformance")]
        public List<AbstractConstraintXml> Constraints;

        [XmlElement("TestCase")]
        public List<TestCaseXml> TestCases;

        public IList<Constraint> Instantiate()
        {
            var list = new List<Constraint>();
            foreach (var constraint in Constraints)
            {
                var c = constraint.Define();
                list.Add(c);
            }
            return list;
        }

        public void Play()
        {
            var ctrs = Instantiate();
            foreach (var c in ctrs)
            {
                foreach (var tc in TestCases)
                {
                    tc.Play(c);
                }
            }
        }
    }
}