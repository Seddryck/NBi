using NBi.Xml.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml;

public class GroupXml : InheritanceTestXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlElement("category", Order = 1)]
    public List<string> Categories
    {
        get { return categories; }
        set { categories = value; }
    }

    [XmlElement("setup", Order = 2)]
    public SetupXml Setup
    {
        get { return setup; }
        set { setup = value; }
    }

    [XmlElement("cleanup", Order = 3)]
    public CleanupXml Cleanup
    {
        get { return cleanup; }
        set { cleanup = value; }
    }

    [XmlElement("test", Order = 4)]
    public List<TestXml> Tests { get; set; }

    [XmlElement("group", Order = 5)]
    public List<GroupXml> Groups { get; set; }

    [XmlIgnore()]
    public IList<string> GroupNames { get; private set; }

    public GroupXml()
        : base()
    {
        Tests = new List<TestXml>();
        Groups = new List<GroupXml>();
        GroupNames = new List<string>();
    }

    public override string ToString()
        => string.IsNullOrEmpty(Name) ? base.ToString() : Name.ToString();

    internal IEnumerable<TestXml> GetAllTests()
    {
        var allTests = new List<TestXml>();
        Tests.ForEach(t => t.AddInheritance(Categories, Setup, Cleanup));
        allTests.AddRange(this.Tests);

        this.GroupNames.Add(this.Name);
        foreach (var group in Groups)
        {
            group.AddInheritance(Categories, Setup, Cleanup);
            foreach (var groupName in this.GroupNames)
                group.GroupNames.Add(groupName);
            allTests.AddRange(group.GetAllTests());
        }

        foreach (var test in Tests)
            foreach (var groupName in this.GroupNames)
                test.GroupNames.Add(groupName);

        return allTests;
    }

    [XmlIgnore]
    public bool SetupSpecified
    {
        get => (Setup?.Commands?.Count ?? 0) != 0;
        set { return; }
    }

    [XmlIgnore]
    public bool CleanupSpecified
    {
        get => (Cleanup?.Commands?.Count ?? 0) != 0;
        set { return; }
    }
}
