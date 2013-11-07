using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml
{
    public class GroupXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("test", Order = 1)]
        public List<TestXml> Tests { get; set; }

        [XmlElement("group", Order = 2)]
        public List<GroupXml> Groups { get; set; }

        public GroupXml()
        {
            Tests = new List<TestXml>();
            Groups = new List<GroupXml>();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
                return base.ToString();
            else
                return Name.ToString();
        }

        internal IEnumerable<TestXml> GetAllTests()
        {
            var allTests = new List<TestXml>();
            allTests.AddRange(this.Tests);
            foreach (var group in Groups)
                allTests.AddRange(group.GetAllTests());

            return allTests;
        }
    }
}
