using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Systems;

[TestFixture]
public class MembersXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_MembersWithLevel()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MembersXml>());
        Assert.That(((MembersXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<LevelXml>());

        var item = (LevelXml)((MembersXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Dimension, Is.EqualTo("dimension"));
        Assert.That(item.Hierarchy, Is.EqualTo("hierarchy"));
        Assert.That(item.Caption, Is.EqualTo("level"));
        Assert.That(item.Perspective, Is.EqualTo("Perspective"));
        Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
    }

    [Test]
    public void Deserialize_SampleFile_AutoCategories()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        var autoCategories = ts.Tests[testNr].Systems[0].GetAutoCategories();

        Assert.That(autoCategories, Has.Member("Dimension 'dimension'"));
        Assert.That(autoCategories, Has.Member("Hierarchy 'hierarchy'"));
        Assert.That(autoCategories, Has.Member("Perspective 'Perspective'"));
        Assert.That(autoCategories, Has.Member("Members"));
    }

    [Test]
    public void Deserialize_SampleFile_MembersWithHierarchy()
    {
        int testNr = 1;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MembersXml>());
        Assert.That(((MembersXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchyXml>());

        var item = (HierarchyXml)((MembersXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Dimension, Is.EqualTo("dimension"));
        Assert.That(item.Caption, Is.EqualTo("hierarchy"));
        Assert.That(item.Perspective, Is.EqualTo("Perspective"));
        Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
    }
    
    [Test]
    public void Deserialize_SampleFile_MembersWithChildrenOf()
    {
        int testNr = 2;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MembersXml>());
        Assert.That(((MembersXml)ts.Tests[testNr].Systems[0]).ChildrenOf, Is.EqualTo("aBc"));
    }

    [Test]
    public void Deserialize_SampleFile_MembersWithExlusions()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MembersXml>());
        Assert.That(((MembersXml)ts.Tests[testNr].Systems[0]).Exclude, Is.TypeOf<ExcludeXml>());

        var exclude = ((MembersXml)ts.Tests[testNr].Systems[0]).Exclude;

        Assert.That(exclude.Items.Count, Is.EqualTo(2));
        Assert.That(exclude.Items, Has.Member("Arizona"));
        Assert.That(exclude.Items, Has.Member("Iowa"));
    }

}
