using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class CountXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_CountExactly()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<CountXml>());
        Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).Exactly, Is.EqualTo(10));
        Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).ExactlySpecified, Is.True);
        Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).LessThanSpecified, Is.False);
        Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).MoreThanSpecified, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_CountMoreAndLess()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<CountXml>());
        Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).MoreThan, Is.EqualTo(10));
        Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).LessThan, Is.EqualTo(15));
        Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).ExactlySpecified, Is.False);
        Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).LessThanSpecified, Is.True);
        Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).MoreThanSpecified, Is.True);
    }

    [Test]
    public void Serialize_OnlyExactlySpecified_MoreThanLessThanNotSet()
    {
        var count = new CountXml
        {
            Exactly = 10
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom<CountXml>(count);

        Assert.That(xml, Does.Contain("exactly"));
        Assert.That(xml, Does.Not.Contain("more-than"));
        Assert.That(xml, Does.Not.Contain("less-than"));
    }

    [Test]
    public void Serialize_LessThanSpecified_LessThanSet()
    {
        var count = new CountXml
        {
            LessThan = 10
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom<CountXml>(count);

        Assert.That(xml, Does.Not.Contain("exactly"));
        Assert.That(xml, Does.Not.Contain("more-than"));
        Assert.That(xml, Does.Contain("less-than"));
    }
}
