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

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class PerspectiveXmlTest
    {
        [Test]
        public void Serialize_PerspectiveXml_NoDefaultAndSettings()
        {
            var perspectiveXml = new PerspectiveXml();
            perspectiveXml.Caption = "My Caption";
            perspectiveXml.Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = "connStr" };
            perspectiveXml.Settings = new SettingsXml()
                {
                    References = new List<ReferenceXml>() 
                    { new ReferenceXml() 
                        { Name = "Bob", ConnectionString = "connStr" } 
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
        }
    }
}
