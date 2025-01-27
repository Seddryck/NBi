using System.IO;
using System.Reflection;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit;

[TestFixture]
public class DescriptionXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_DescriptionAttributeNotSpecified_NoDescription()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].Description, Is.Empty);
    }

    [Test]
    public void Deserialize_DescriptionAttributeSet_DescriptionAvailable()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].Description, Is.EqualTo("Test's description"));
    }

    [Test]
    public void Deserialize_DescriptionElementAvailable_DescriptionAvailable()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].Description, Is.EqualTo("Test's description"));
    }
}
