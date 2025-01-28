using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml;

[XmlRoot(ElementName = "test", Namespace = "")]
public class TestStandaloneXml : TestXml
{
    public TestStandaloneXml()
    { }

    public TestStandaloneXml(TestXml full)
        : base()
    {
        Categories = full.Categories;
        Cleanup = full.Cleanup;
        Condition = full.Condition;
        Constraints = full.Constraints;
        DescriptionElement = full.DescriptionElement;
        Drafts = full.Drafts;
        Edition = full.Edition;
        IgnoreElement = full.IgnoreElement;
        InstanceSettling = full.InstanceSettling;
        Name = full.Name;
        Setup = full.Setup;
        Systems = full.Systems;
        Traits = full.Traits;
        UniqueIdentifier = full.UniqueIdentifier;
    }
}
