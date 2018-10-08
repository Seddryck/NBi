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

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class PerspectiveXmlTest
    {

        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.PerspectiveXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }


        [Test]
        public void Serialize_PerspectiveXml_NoDefaultAndSettings()
        {
            var perspectiveXml = new PerspectiveXml()
            {
                Caption = "My Caption",
                Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = "connStr" } },
                Settings = new SettingsXml()
                {
                    References = new List<ReferenceXml>()
                        { new ReferenceXml()
                            { Name = "Bob", ConnectionString = new ConnectionStringXml() { Inline = "connStr" }}
                        }
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

            Assert.That(content, Is.StringContaining("My Caption"));
            Assert.That(content, Is.Not.StringContaining("efault"));
            Assert.That(content, Is.Not.StringContaining("eference"));
            Assert.That(content, Is.Not.StringContaining("owner"));
        }

        [Test]
        public void Deserialize_SampleFile_PerspectiveLoaded()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

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
            TestSuiteXml ts = DeserializeSample();

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
            TestSuiteXml ts = DeserializeSample();

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
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<PerspectivesXml>());

            var item = (PerspectivesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Owner, Is.EqualTo("dbo"));
            Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
        }
    }
}
