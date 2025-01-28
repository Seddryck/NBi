using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Items;

[TestFixture]
public class SetXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_SetLoaded()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<SetXml>());

        var item = (SetXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Perspective, Is.EqualTo("perspective"));
        Assert.That(item.Caption, Is.EqualTo("set"));
    }

    [Test]
    public void Deserialize_SampleFile_SetsLoaded()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<SetsXml>());

        var item = (SetsXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Perspective, Is.EqualTo("perspective"));
    }

    [Test]
    public void Deserialize_SampleWithMembersFile_SetsLoaded()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MembersXml>());
        Assert.That(((MembersXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<SetXml>());

        var item = (SetXml)((MembersXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Perspective, Is.EqualTo("perspective"));
        Assert.That(item.Caption, Is.EqualTo("set"));
    }      
}
