using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Settings;
using NBi.Xml.Variables;

namespace NBi.Xml;

[XmlRoot(ElementName = "testSuite", Namespace = "http://NBi/TestSuite")]
public class TestSuiteXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }
    
    [XmlElement("settings", Order = 1)]
    public SettingsXml Settings { get; set; }

    [XmlArray(ElementName = "variables", Order = 2)]
    [XmlArrayItem(typeof(GlobalVariableXml), ElementName = "variable")]
    public List<GlobalVariableXml> Variables { get; set; }

    [XmlIgnore]
    public bool VariablesSpecified { get => Variables != null && Variables.Count > 0; }

    [XmlElement("test", Order = 3)]
    public List<TestXml> Tests { get; set; }

    [XmlElement("group", Order = 4)]
    public List<GroupXml> Groups { get; set; }

    public TestSuiteXml()
    {
        Tests = new List<TestXml>();
        Groups = new List<GroupXml>();
        Settings = new SettingsXml();
        Variables = new List<GlobalVariableXml>();
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(Name))
            return base.ToString();
        else
            return Name.ToString();
    }

    public void Load(IEnumerable<TestXml> tests)
    {
        foreach (var test in tests)
        {
            if (test is TestStandaloneXml)
            {
                var t = new TestXml((TestStandaloneXml)test);
                this.Tests.Add(t);
            }
            else
                this.Tests.Add(test);
        }
    }

    public IEnumerable<TestXml> GetAllTests()
    {
        var allTests = new List<TestXml>();
        allTests.AddRange(this.Tests);
        foreach (var group in Groups)
            allTests.AddRange(group.GetAllTests());

        return allTests;
    }

    

}
