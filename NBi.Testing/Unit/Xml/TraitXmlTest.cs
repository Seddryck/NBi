﻿using System.IO;
using System.Reflection;
using NBi.Xml;
using NUnit.Framework;
using NBi.Xml.Settings;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class TraitXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.TraitXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_TraitAttributeNotSpecified_NoTrait()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Traits, Has.Count.EqualTo(0));
        }

        [Test]
        public void Deserialize_TraitAttributeSet_OneTrait()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Traits, Has.Count.EqualTo(1));
            var firstTrait = ts.Tests[testNr].Traits[0];
            Assert.That(firstTrait.Name, Is.EqualTo("My Property One"));
            Assert.That(firstTrait.Value, Is.EqualTo("My Value"));
            
        }

        [Test]
        public void Deserialize_TraitAttributeSetTwice_TwoTraits()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Traits, Has.Count.EqualTo(2));
        }

        [Test]
        public void Serialize_Trait_NameAsAttributeValueAsText()
        {
            var test = new TestXml();
            var trait = new TraitXml() { Name = "My Trait", Value = "My Trait's value" };
            test.Traits.Add(trait);
            
            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<TestXml>(test);

            Assert.That(xml, Is.StringContaining("<trait name=\"My Trait\">My Trait's value</trait>"));
        }
    }
}
