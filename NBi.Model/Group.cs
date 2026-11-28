using NBi.Model.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model;

public class Group : InheritanceTest
{
    public string Name { get; set; }

    public List<string> Categories
    {
        get { return categories; }
        set { categories = value; }
    }

    public Setup Setup
    {
        get { return setup; }
        set { setup = value; }
    }

    public Cleanup Cleanup
    {
        get { return cleanup; }
        set { cleanup = value; }
    }

    public List<TestDefinition> Tests { get; set; }

    public List<Group> Groups { get; set; }

    public IList<string> GroupNames { get; private set; }

    public Group()
        : base()
    {
        Tests = new List<TestDefinition>();
        Groups = new List<Group>();
        GroupNames = new List<string>();
    }

    public override string ToString()
        => string.IsNullOrEmpty(Name) ? base.ToString() : Name.ToString();

    internal IEnumerable<TestDefinition> GetAllTests()
    {
        var allTests = new List<TestDefinition>();
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
        {
            foreach (var groupName in this.GroupNames)
                test.GroupNames.Add(groupName);
        }

        return allTests;
    }

    public bool SetupSpecified
    {
        get => (Setup?.Commands?.Count ?? 0) != 0;
        set { return; }
    }

    public bool CleanupSpecified
    {
        get => (Cleanup?.Commands?.Count ?? 0) != 0;
        set { return; }
    }
}
