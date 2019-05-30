using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.SerializationOption;
using NBi.Xml.Systems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Xml.Items
{
    public class FileXmlTest
    {
        [Test]
        public void Serialize_JustFileName_NoElementForParser()
        {
            var root = new ResultSetSystemXml()
            {
                File = new FileXml
                {
                    Path = "c:\\myFile.txt",
                }
            };

            var overrides = new WriteOnlyAttributes();
            overrides.Build();

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root, overrides);
            Console.WriteLine(xml);
            Assert.That(xml, Is.StringContaining("<file>"));
            Assert.That(xml, Is.StringContaining("<path>c:\\myFile.txt</path>"));
            Assert.That(xml, Is.StringContaining("</file>"));
            Assert.That(xml, Is.Not.StringContaining("<parser"));
            Assert.That(xml, Is.Not.StringContaining("<if-missing"));
        }

        [Test]
        public void Serialize_FileWithParser_NoAttributeTwoElements()
        {
            var root = new ResultSetSystemXml()
            {
                File = new FileXml
                {
                    Path = "c:\\myFile.txt",
                    Parser = new ParserXml() { Name = "myName" }
                }
            };

            var overrides = new WriteOnlyAttributes();
            overrides.Build();

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root, overrides);
            Console.WriteLine(xml);
            Assert.That(xml, Is.StringContaining("<file>"));
            Assert.That(xml, Is.StringContaining("<path>c:\\myFile.txt</path>"));
            Assert.That(xml, Is.StringContaining("<parser name=\"myName\" />"));
            Assert.That(xml, Is.StringContaining("</file>"));
        }

        [Test]
        public void Serialize_FileWithIfMissing_NoAttributeTwoElements()
        {
            var root = new ResultSetSystemXml()
            {
                File = new FileXml
                {
                    Path = "c:\\myFile.txt",
                    IfMissing = new IfMissingXml() { File = new FileXml() { Path = "C:\\myOtherFile.txt"} },
                }
            };

            var overrides = new WriteOnlyAttributes();
            overrides.Build();

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root, overrides);
            Console.WriteLine(xml);
            Assert.That(xml, Is.StringContaining("<file>"));
            Assert.That(xml, Is.StringContaining("<path>c:\\myFile.txt</path>"));
            Assert.That(xml, Is.StringContaining("<if-missing"));
            Assert.That(xml, Is.StringContaining(">C:\\myOtherFile.txt<"));
            Assert.That(xml, Is.StringContaining("</file>"));
        }
    }
}
