using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NUnit.Framework;
using NBi.Xml;
using System.Reflection;
using NBi.Xml.Systems;

namespace NBi.Xml.Testing.Unit.Items;

[TestFixture]
public class PerspectiveXmlTest : BaseXmlTest
{
    [Test]
    public void Serialize_PerspectiveXml_WithDefaultAndSettings()
    {
        var perspectiveXml = new PerspectiveXml()
        {
            Caption = "My Caption",
            Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = "connStr" } },
            Settings = new SettingsXml()
            {
                References =
                    [ new ReferenceXml()
                        { Name = "Bob", ConnectionString = new ConnectionStringXml() { Inline = "connStr" }}
                    ]
            }
        };

        var serializer = new XmlSerializer(typeof(PerspectiveXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, perspectiveXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("My Caption"));
        Assert.That(content, Does.Not.Contain("efault"));
        Assert.That(content, Does.Not.Contain("eference"));
        Assert.That(content, Does.Not.Contain("owner"));
    }

    [Test]
    public void Serialize_PerspectiveXml_WithoutDefaultAndSettings()
    {
        var perspectiveXml = new PerspectiveXml()
        {
            Caption = "My Caption",
            ConnectionString = "connStr"
        };

        var serializer = new XmlSerializer(typeof(PerspectiveXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, perspectiveXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("My Caption"));
        Assert.That(content, Does.Not.Contain("connectionString"));
        Assert.That(content, Does.Contain("connection-string"));
    }

    [Test]
    public void Deserialize_SampleFile_PerspectiveLoaded()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<PerspectiveXml>());

        var item = (PerspectiveXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Caption, Is.EqualTo("Perspective"));
        Assert.That(item.Owner, Is.Null.Or.Empty);
        Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
    }

    [Test]
    public void Deserialize_SampleFile_PerspectiveAndOwnerLoaded()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<PerspectiveXml>());

        var item = (PerspectiveXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Caption, Is.EqualTo("Perspective"));
        Assert.That(item.Owner, Is.EqualTo("dbo"));
        Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
    }

    [Test]
    public void Deserialize_SampleFile_PerspectivesLoaded()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<PerspectivesXml>());

        var item = (PerspectivesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Owner, Is.Null.Or.Empty);
        Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
    }

    [Test]
    public void Deserialize_SampleFile_PerspectivesAndOwnerLoaded()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
        Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<PerspectivesXml>());

        var item = (PerspectivesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Owner, Is.EqualTo("dbo"));
        Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
    }
}
