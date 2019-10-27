using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Items.Hierarchical.Json;
using NBi.Xml.Constraints;
using NUnit.Framework;
using System;

namespace NBi.Testing.Xml.Unit.Items.Hierarchical
{
    public class JsonSourceXmlTest : BaseXmlTest
    {
        [Test]
        public void Deserialize_SampleFile_XmlSource()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            Assert.That(((ts.Tests[testNr].Constraints[0]) as EqualToXml).ResultSet.JsonSource, Is.TypeOf<JsonSourceXml>());
        }

        [Test]
        public void Deserialize_SampleFile_File()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var jsonSource = ((ts.Tests[testNr].Constraints[0]) as EqualToXml).ResultSet.JsonSource;
            Assert.That(jsonSource.File, Is.TypeOf<FileXml>());
            Assert.That(jsonSource.File.Path, Is.Not.Empty.And.Not.Null);
            Assert.That(jsonSource.File.Path, Is.EqualTo("Myfile.json"));
        }

        [Test]
        public void Serialize_File_PathIsSet()
        {
            var root = new JsonSourceXml()
            {
                File = new FileXml
                {
                    Path = "C:\\myPath.json"
                }
            };
            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(root);
            Console.WriteLine(xml);
            Assert.That(xml, Does.Contain("<file>"));
            Assert.That(xml, Does.Contain("<path>"));
            Assert.That(xml, Does.Contain("C:\\myPath.json"));
            Assert.That(xml, Does.Not.Contain("<parser"));
        }

        [Test]
        public void Deserialize_SampleFile_JsonPath()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var jsonSource = ((ts.Tests[testNr].Constraints[0]) as EqualToXml).ResultSet.JsonSource;
            Assert.That(jsonSource.JsonPath, Is.TypeOf<JsonPathXml>());
        }

        [Test]
        public void Deserialize_SampleFile_XPathFrom()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var jsonPath = ((ts.Tests[testNr].Constraints[0]) as EqualToXml).ResultSet.JsonSource.JsonPath;
            Assert.That(jsonPath.From, Is.TypeOf<JsonFromXml>());
            Assert.That(jsonPath.From.Value, Is.EqualTo("$.Path[*]"));
        }

        [Test]
        public void Deserialize_SampleFile_JsonPathSelects()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var jsonPath = ((ts.Tests[testNr].Constraints[0]) as EqualToXml).ResultSet.JsonSource.JsonPath;
            Assert.That(jsonPath.Selects, Is.Not.Null.And.Not.Empty);
            Assert.That(jsonPath.Selects, Has.Count.EqualTo(2));
        }

        [Test]
        public void Deserialize_SampleFile_XPathSelectElement()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var selects = ((ts.Tests[testNr].Constraints[0]) as EqualToXml).ResultSet.JsonSource.JsonPath.Selects;
            Assert.That(selects[0].Value, Is.EqualTo("$.Item.SubItem[*].Quantity"));
            Assert.That(selects[1].Value, Is.EqualTo("!.Number"));
        }
    }
}
