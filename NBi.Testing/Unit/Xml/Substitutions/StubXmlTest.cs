using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
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
    public class StubXmlTest
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
        public void Deserialize_Stub_Deserialised()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Substitutions[0], Is.InstanceOf<AbstractSubstitutionXml>());
            Assert.That(ts.Tests[testNr].Substitutions[0], Is.TypeOf<StubXml>());
        }

        [Test]
        public void Deserialize_DatabaseObject_Deserialized()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var stub = ts.Tests[testNr].Substitutions[0] as StubXml;
            Assert.That(stub.DatabaseObject, Is.TypeOf<DatabaseObjectXml>());
        }

        [Test]
        public void Deserialize_ResultSet_Deserialized()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var stub = ts.Tests[testNr].Substitutions[0] as StubXml;
            Assert.That(stub.Return, Is.TypeOf<ReturnXml>());
        }
    }
}
