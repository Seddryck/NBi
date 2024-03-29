﻿using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Items.Hierarchical.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
using System;

namespace NBi.Testing.Xml.Unit.Items.Hierarchical
{
    public class XmlSourceXmlTest : BaseXmlTest
    {
        [Test]
        public void Deserialize_SampleFile_XmlSource()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).BaseItem, Is.TypeOf<XmlSourceXml>());
        }

        [Test]
        public void Deserialize_SampleFile_File()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var xmlSource = (ts.Tests[testNr].Constraints[0]).BaseItem as XmlSourceXml;
            Assert.That(xmlSource.File, Is.TypeOf<FileXml>());
            Assert.That(xmlSource.File.Path, Is.Not.Empty.And.Not.Null);
            Assert.That(xmlSource.File.Path, Is.EqualTo("Myfile.csv"));
        }

        [Test]
        public void Deserialize_SampleFileWithPath_File()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var xmlSource = (ts.Tests[testNr].Constraints[0]).BaseItem as XmlSourceXml;
            Assert.That(xmlSource.File, Is.TypeOf<FileXml>());
            Assert.That(xmlSource.File.Path, Is.Not.Empty.And.Not.Null);
            Assert.That(xmlSource.File.Path, Is.EqualTo("Myfile.csv"));
        }

        [Test]
        public void Serialize_File_PathIsSet()
        {
            var root = new XmlSourceXml()
            {
                File = new FileXml
                {
                    Path = "C:\\myPath.txt"
                }
            };
            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root);
            Assert.That(xml, Does.Contain("<file>"));
            Assert.That(xml, Does.Contain("<path>"));
            Assert.That(xml, Does.Contain("C:\\myPath.txt"));
            Assert.That(xml, Does.Not.Contain("<parser"));
        }

        [Test]
        public void Serialize_File_ParserIsSet()
        {
            var root = new XmlSourceXml()
            {
                File = new FileXml
                {
                    Path = "C:\\myPath.txt",
                    Parser = new ParserXml() { Name = "myName" },
                },
            };
            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root);
            Assert.That(xml, Does.Contain("<parser "));
            Assert.That(xml, Does.Contain("name=\"myName\""));
        }

        [Test]
        public void Deserialize_SampleFile_XPath()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var xmlSource = (ts.Tests[testNr].Constraints[0]).BaseItem as XmlSourceXml;
            Assert.That(xmlSource.XPath, Is.TypeOf<XPathXml>());
        }

        [Test]
        public void Deserialize_SampleFile_XPathFrom()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var xpath = ((ts.Tests[testNr].Constraints[0]).BaseItem as XmlSourceXml).XPath;
            Assert.That(xpath.From, Is.TypeOf<FromXml>());
            Assert.That(xpath.From.Value, Is.EqualTo("//Path"));
        }

        [Test]
        public void Deserialize_SampleFile_XPathSelects()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var xpath = ((ts.Tests[testNr].Constraints[0]).BaseItem as XmlSourceXml).XPath;
            Assert.That(xpath.Selects, Is.Not.Null.And.Not.Empty);
            Assert.That(xpath.Selects, Has.Count.EqualTo(2));
        }

        [Test]
        public void Deserialize_SampleFile_XPathSelectAttribute()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var select = ((ts.Tests[testNr].Constraints[0]).BaseItem as XmlSourceXml).XPath.Selects[0];
            Assert.That(select.Attribute, Is.EqualTo("Id"));
            Assert.That(select.Value, Is.EqualTo("//Path/Item"));
        }

        [Test]
        public void Deserialize_SampleFile_XPathSelectElement()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var select = ((ts.Tests[testNr].Constraints[0]).BaseItem as XmlSourceXml).XPath.Selects[1];
            Assert.That(select.Attribute, Is.Null);
            Assert.That(select.Value, Is.EqualTo("//Path/Item/SubItem"));
        }
    }
}
