using System.Collections.Generic;
using NBi.Model.Settings;
using NBi.Model.Variables;

namespace NBi.Model;

public class TestSuite
{
    public string Name { get; set; }

    public SuiteSettings Settings { get; set; }

    public List<GlobalVariable> Variables { get; set; }

    public bool VariablesSpecified { get => Variables != null && Variables.Count > 0; }

    public List<TestDefinition> Tests { get; set; }

    public List<Group> Groups { get; set; }

    public TestSuite()
    {
        Tests = new List<TestDefinition>();
        Groups = new List<Group>();
        Settings = new SuiteSettings();
        Variables = new List<GlobalVariable>();
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(Name))
            return base.ToString();
        else
            return Name.ToString();
    }

    public void Load(IEnumerable<TestDefinition> tests)
    {
        foreach (var test in tests)
        {
            if (test is StandaloneTest)
            {
                var t = new TestDefinition((StandaloneTest)test);
                this.Tests.Add(t);
            }
            else
                this.Tests.Add(test);
        }
    }

    public IEnumerable<TestDefinition> GetAllTests()
    {
        var allTests = new List<TestDefinition>();
        allTests.AddRange(this.Tests);
        foreach (var group in Groups)
            allTests.AddRange(group.GetAllTests());

        return allTests;
    }

}
