using System.IO;
using System.Reflection;
using NBi.Xml;
using NUnit.Framework;
using NBi.Xml.Settings;

namespace NBi.Xml.Testing.Unit;

[TestFixture]
public class TraitXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_TraitAttributeNotSpecified_NoTrait()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].Traits, Has.Count.EqualTo(0));
    }

    [Test]
    public void Deserialize_TraitAttributeSet_OneTrait()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].Traits, Has.Count.EqualTo(1));
        var firstTrait = ts.Tests[testNr].Traits[0];
        Assert.That(firstTrait.Name, Is.EqualTo("My Property One"));
        Assert.That(firstTrait.Value, Is.EqualTo("My Value"));
        
    }

    [Test]
    public void Deserialize_TraitAttributeSetTwice_TwoTraits()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].Traits, Has.Count.EqualTo(2));
    }

    [Test]
    public void Serialize_Trait_NameAsAttributeValueAsText()
    {
        var test = new TestXml();
        var trait = new TraitXml() { Name = "My Trait", Value = "My Trait's value" };
        test.Traits.Add(trait);
        
        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom<TestXml>(test);

        Assert.That(xml, Does.Contain("<trait name=\"My Trait\">My Trait's value</trait>"));
    }
}
