using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Substitutions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Xml.Substitutions
{
    public class FakeXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.SubstitutionXmlTestSuite.xml"))
            using (var reader = new StreamReader(stream))
                manager.Read(reader);

            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_Fake_Deserialised()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Substitutions[0], Is.InstanceOf<AbstractSubstitutionXml>());
            Assert.That(ts.Tests[testNr].Substitutions[0], Is.TypeOf<FakeXml>());
        }

        [Test]
        public void Deserialize_DatabaseObject_Deserialized()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var fake = ts.Tests[testNr].Substitutions[0] as FakeXml;
            Assert.That(fake.DatabaseObject, Is.TypeOf<DatabaseObjectXml>());
        }

        [Test]
        public void Deserialize_Code_Deserialized()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var fake = ts.Tests[testNr].Substitutions[0] as FakeXml;
            Assert.That(fake.Code, Is.EqualTo("select * from DimCustomers;"));
        }
    }
}
