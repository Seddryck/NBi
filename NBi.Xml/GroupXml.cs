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

        [XmlElement("category", Order = 1)]
        public List<string> Categories;

        [XmlElement("test", Order = 2)]
        public List<TestXml> Tests { get; set; }

        [XmlElement("group", Order = 3)]
        public List<GroupXml> Groups { get; set; }

        [XmlIgnore()]
        public IList<string> GroupNames { get; private set; }

        public GroupXml()
        {
            Categories = new List<string>();
            Tests = new List<TestXml>();
            Groups = new List<GroupXml>();
            GroupNames = new List<string>();
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
            Tests.ForEach(t => t.AddInheritedCategories(Categories));
            allTests.AddRange(this.Tests);

            this.GroupNames.Add(this.Name);
            foreach (var group in Groups)
            {
                group.AddInheritedCategories(Categories);
                foreach (var groupName in this.GroupNames)
                    group.GroupNames.Add(groupName);
                allTests.AddRange(group.GetAllTests());
            }

            foreach (var test in Tests)
                foreach (var groupName in this.GroupNames)
                    test.GroupNames.Add(groupName);

            return allTests;
        }

        internal void AddInheritedCategories(List<string> categories)
        {
            Categories.AddRange(categories);
        }

    }
}
