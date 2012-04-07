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

        [XmlElement(Type = typeof(SyntacticallyCorrectXml), ElementName = "SyntacticallyCorrect"),
        XmlElement(Type = typeof(FasterThanXml), ElementName = "FasterThan")]
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