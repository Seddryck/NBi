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
public class DataTypeXmlTest : BaseXmlTest
{ 
    [Test]
    public void Deserialize_SampleFile_Column()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<DataTypeXml>());
        Assert.That(((DataTypeXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<ColumnXml>());

        var item = (ColumnXml)((DataTypeXml)ts.Tests[testNr].Systems[0]).Item;
        Assert.That(item.Caption, Is.EqualTo("column"));
        Assert.That(item.Perspective, Is.EqualTo("dwh"));
        Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
    }


    [Test]
    public void Serialize_DataTypeXml_NoDefaultAndSettings()
    {
        var columnXml = new ColumnXml()
        {
            Caption = "My Caption",
            Perspective = "My Schema",
            Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = "connStr" } },
            Settings = new SettingsXml()
            {
                References =
                [ new ReferenceXml()
                    { Name = "Bob", ConnectionString = new ConnectionStringXml() { Inline = "connStr" } }
                ]
            }
        };
        var dataTypeXml = new DataTypeXml() { Item = columnXml };

        var serializer = new XmlSerializer(typeof(DataTypeXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, dataTypeXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<column"));
        Assert.That(content, Does.Contain("caption=\"My Caption\""));
        Assert.That(content, Does.Contain("perspective=\"My Schema\""));
        Assert.That(content, Does.Not.Contain("efault"));
        Assert.That(content, Does.Not.Contain("eference"));
    }


}
