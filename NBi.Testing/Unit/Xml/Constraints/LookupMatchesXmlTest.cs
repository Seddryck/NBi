using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.ResultSet.Lookup;
using NBi.Xml.Systems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class LookupMatchesXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.LookupMatchesXmlTestSuite.xml"))
            using (var reader = new StreamReader(stream))
                manager.Read(reader);
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyReferenceExists()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<LookupMatchesXml>());
            Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyJoinMapping()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var lookupMatches = ts.Tests[testNr].Constraints[0] as LookupMatchesXml;
            var mappings = lookupMatches.Join.Mappings;

            Assert.That(mappings, Has.Count.EqualTo(1));
            Assert.That(mappings[0].Candidate, Is.EqualTo("DepartmentID"));
            Assert.That(mappings[0].Reference, Is.EqualTo("Id"));
            Assert.That(mappings[0].Type, Is.EqualTo(ColumnType.Numeric));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyInclusionMapping()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var lookupMatches = ts.Tests[testNr].Constraints[0] as LookupMatchesXml;
            var mappings = lookupMatches.Inclusion.Mappings;
             
            Assert.That(mappings, Has.Count.EqualTo(1));
            Assert.That(mappings[0].Candidate, Is.EqualTo("DepartmentName"));
            Assert.That(mappings[0].Reference, Is.EqualTo("Name"));
            Assert.That(mappings[0].Type, Is.EqualTo(ColumnType.Text));
        }
    }
}
