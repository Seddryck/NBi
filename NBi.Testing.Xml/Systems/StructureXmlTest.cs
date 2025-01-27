#region Using directives
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Xml.Testing.Unit.Systems;

[TestFixture]
public class StructureXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_Hierarchy()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        
        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchyXml>());

        var item = (HierarchyXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Caption, Is.EqualTo("hierarchy"));
        Assert.That(item.Dimension, Is.EqualTo("dimension"));
        Assert.That(item.Perspective, Is.EqualTo("Perspective"));
        Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
    }


    [Test]
    public void GetAutoCategories_Hierarchy_ValidList()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        var autoCategories = ts.Tests[testNr].Systems[0].GetAutoCategories();

        Assert.That(autoCategories, Has.Member("Dimension 'dimension'"));
        Assert.That(autoCategories, Has.Member("Perspective 'Perspective'"));
        Assert.That(autoCategories, Has.Member("Hierarchy 'hierarchy'"));
        Assert.That(autoCategories, Has.Member("Hierarchies"));
        Assert.That(autoCategories, Has.Member("Structure"));
    }

    [Test]
    public void GetAutoCategories_MeasureGroup_ValidList()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        var autoCategories = ts.Tests[testNr].Systems[0].GetAutoCategories();

        Assert.That(autoCategories, Has.Member("Measure group 'MeasureGroupName'"));
        Assert.That(autoCategories, Has.Member("Perspective 'Perspective'"));
        Assert.That(autoCategories, Has.Member("Measure groups"));
        Assert.That(autoCategories, Has.Member("Structure"));
    }

    [Test]
    public void Deserialize_SampleFile_MeasureGroup()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<MeasureGroupXml>());

        var item = (MeasureGroupXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Perspective, Is.EqualTo("Perspective"));
        Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
    }

    [Test]
    public void Serialize_StructureXml_NoDefaultAndSettings()
    {
        var perspectiveXml = new PerspectiveXml
        {
            Caption = "My Caption",
            Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = "connStr" } },
            Settings = new SettingsXml()
            {
                References =
                [ new ReferenceXml()
                    { Name = "Bob", ConnectionString = new ConnectionStringXml() { Inline = "connStr" } }
                ]
            }
        };
        var structureXml = new StructureXml() { Item = perspectiveXml };

        var serializer = new XmlSerializer(typeof(StructureXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, structureXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("My Caption"));
        Assert.That(content, Does.Not.Contain("efault"));
        Assert.That(content, Does.Not.Contain("eference"));
    }
    

}
