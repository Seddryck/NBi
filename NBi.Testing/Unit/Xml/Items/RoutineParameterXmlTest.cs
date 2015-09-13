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
using NBi.Xml.Items.Filters;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class RoutineParameterXmlTest
    {
        [Test]
        public void Serialize_ParameterXml_Serialize()
        {
            var structureXml = new StructureXml();
            var parameterXml = new RoutineParameterXml();
            parameterXml.Caption = "My Caption";
            parameterXml.Perspective = "My Perspective";
            parameterXml.Routine = "My Routine";
            structureXml.Item = parameterXml;

            var serializer = new XmlSerializer(typeof(StructureXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, structureXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("caption=\"My Caption\""));
            Assert.That(content, Is.StringContaining("perspective=\"My Perspective\""));
            Assert.That(content, Is.StringContaining("routine=\"My Routine\""));
            Assert.That(content, Is.Not.StringContaining("direction"));
            Assert.That(content, Is.Not.StringContaining("result"));
            Assert.That(content, Is.StringContaining("<parameter"));
        }
        [Test]
        public void Serialize_ParameterWithOptionalXml_Serialize()
        {
            var structureXml = new StructureXml();
            var parameterXml = new RoutineParameterXml();
            parameterXml.Caption = "My Caption";
            parameterXml.Perspective = "My Perspective";
            parameterXml.Routine = "My Routine";
            parameterXml.Direction = ParameterDirectionOption.In;
            parameterXml.IsResult = IsResultOption.Yes;
            structureXml.Item = parameterXml;

            var serializer = new XmlSerializer(typeof(StructureXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, structureXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("result=\"yes\""));
            Assert.That(content, Is.StringContaining("direction=\"in\""));
        }
    }
}
