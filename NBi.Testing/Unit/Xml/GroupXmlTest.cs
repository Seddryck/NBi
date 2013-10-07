using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class GroupXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.GroupXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_TestSuiteLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Name, Is.EqualTo("The TestSuite"));
        }

        [Test]
        public void Deserialize_SampleFile_TestsLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests.Count, Is.EqualTo(2));
        }

        [Test]
        public void Deserialize_SampleFile_GroupsLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Groups.Count, Is.EqualTo(2));
        }

        [Test]
        public void Deserialize_SampleFile_TestMembersLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Groups[0].Name, Is.EqualTo("My first tests' group"));
            Assert.That(ts.Groups[0].Tests[0].UniqueIdentifier, Is.EqualTo("0001"));
            Assert.That(ts.Groups[0].Tests[1].UniqueIdentifier, Is.EqualTo("0002"));
        }

        [Test]
        public void Deserialize_SampleFile_GroupMembersLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Groups[1].Name, Is.EqualTo("My second tests' group"));
            Assert.That(ts.Groups[1].Tests.Count, Is.EqualTo(1));
            Assert.That(ts.Groups[0].Groups.Count, Is.EqualTo(0));
            Assert.That(ts.Groups[1].Groups.Count, Is.EqualTo(1));
        }


    }
}
