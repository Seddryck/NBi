using System.IO;
using System.Reflection;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit;

[TestFixture]
public class IgnoreXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_IgnoreAttributeNotSpecified_NotIgnored()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].Ignore, Is.False);
    }

    [Test]
    public void Deserialize_IgnoreAttributeSetToTrue_Ignored()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].Ignore, Is.True);
    }

    [Test]
    public void Deserialize_IgnoreElementAvailable_Ignored()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].Ignore, Is.True);
    }

    [Test]
    public void Deserialize_IgnoreReasonFilled_IgnoreReasonLoaded()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
        Assert.That(ts.Tests[testNr].IgnoreReason, Is.EqualTo("The reason to ignore this test."));
    }

}
