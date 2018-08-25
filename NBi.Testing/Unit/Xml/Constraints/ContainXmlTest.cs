using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using NBi.Service;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class ContainXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.ContainXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_ContainCaptionNotIgnoringCaseImplicitely()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainXml>());
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(1));
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items[0].ToLower(), Is.EqualTo("xyz"));
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ContainCaptionIgnoringCaseExplicitely()
        {
            int testNr = 1;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(1));
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items[0].ToLower(), Is.EqualTo("xyz"));
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_ContainReadItems()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(2));
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items[0], Is.EqualTo("xyz"));
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items[1], Is.EqualTo("abc"));
        }


        [Test]
        public void Serialize_ContainWithCaption_ContainItems()
        {
            var containXml = new ContainXml
            {
#pragma warning disable 0618
                Caption = "myMember"
#pragma warning restore 0618
            };

            var serializer = new XmlSerializer(typeof(ContainXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, containXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.Not.StringContaining("caption"));
            Assert.That(content, Is.StringContaining("<item>"));
            Assert.That(content, Is.StringContaining("myMember"));
        }

        [Test]
        public void ReSerialize_ContainWithCaption_ContainItems()
        {
            var template = "<test><system-under-test><members><level/></members></system-under-test><assert><contain caption=\"$member$\"/></assert></test>";
            var engine = new StringTemplateEngine(template, new string[] { "member" });
            var list = new List<List<List<object>>>() { new List<List<object>>() { new List<object>() { "myMember" } } };
            var tests = engine.Build(list, null);
            var content = tests.ElementAt(0).Content;
            Assert.That(content, Is.StringContaining("<item>"));
            Assert.That(content, Is.StringContaining("myMember"));
            Assert.That(content, Is.Not.StringContaining("caption"));
        }
    }
}
