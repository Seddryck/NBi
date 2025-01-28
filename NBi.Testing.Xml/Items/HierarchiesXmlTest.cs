using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Items;

[TestFixture]
public class HierarchiesXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_DimensionLoaded()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchiesXml>());

        var item = (HierarchiesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Dimension, Is.EqualTo("dimension"));
    }

    [Test]
    public void Deserialize_SampleFile_ConnectionStringLoaded()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchiesXml>());

        var item = (HierarchiesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.ConnectionString, Is.EqualTo("connStr"));
    }

    [Test]
    public void Deserialize_SampleFile_DisplayFolderLoaded()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchiesXml>());

        var item = (HierarchiesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.DisplayFolder, Is.EqualTo("display-folder"));
        Assert.That(item.Specification.IsDisplayFolderSpecified, Is.True);
    }

    [Test]
    public void Deserialize_SampleFile_DisplayFolderNotSpecified()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchiesXml>());

        var item = (HierarchiesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.DisplayFolder, Is.Null.Or.Empty);
        Assert.That(item.Specification.IsDisplayFolderSpecified, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_MeasureWithDisplayFolderRoot()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchiesXml>());

        var item = (HierarchiesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.DisplayFolder, Is.Empty);
        Assert.That(item.Specification.IsDisplayFolderSpecified, Is.True);
    }

           
}
