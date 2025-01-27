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
using NBi.Xml.Systems;

namespace NBi.Xml.Testing.Unit.Items;

[TestFixture]
public class RoutineXmlTest
{
    [Test]
    public void Serialize_RoutineXml_Serialize()
    {
        var structureXml = new StructureXml();
        var routineXml = new RoutineXml
        {
            Caption = "My Caption",
            Perspective = "My Perspective"
        };
        structureXml.Item = routineXml;

        var serializer = new XmlSerializer(typeof(StructureXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, structureXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("caption=\"My Caption\""));
        Assert.That(content, Does.Contain("perspective=\"My Perspective\""));
        Assert.That(content, Does.Contain("<routine"));
    }
}
