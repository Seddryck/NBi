using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Xml.Systems.Structure;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class DeserializeMembers
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.TestSuiteMembers.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_MembersWithLevel()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<LevelXml>());
            Assert.That(((LevelXml)ts.Tests[testNr].Systems[0]).Dimension, Is.EqualTo("dimension"));
            Assert.That(((LevelXml)ts.Tests[testNr].Systems[0]).Hierarchy, Is.EqualTo("hierarchy"));
            Assert.That(((LevelXml)ts.Tests[testNr].Systems[0]).Caption, Is.EqualTo("level"));
            Assert.That(((LevelXml)ts.Tests[testNr].Systems[0]).Perspective, Is.EqualTo("Perspective"));
            Assert.That(((LevelXml)ts.Tests[testNr].Systems[0]).ConnectionString, Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Deserialize_SampleFile_MembersWithHierarchy()
        {
            int testNr = 1;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<HierarchyXml>());
            Assert.That(((HierarchyXml)ts.Tests[testNr].Systems[0]).Dimension, Is.EqualTo("dimension"));
            Assert.That(((HierarchyXml)ts.Tests[testNr].Systems[0]).Caption, Is.EqualTo("hierarchy"));
            Assert.That(((HierarchyXml)ts.Tests[testNr].Systems[0]).Perspective, Is.EqualTo("Perspective"));
            Assert.That(((HierarchyXml)ts.Tests[testNr].Systems[0]).ConnectionString, Is.EqualTo("ConnectionString"));
        }
        
        [Test]
        public void Deserialize_SampleFile_MembersWithChildrenOf()
        {
            int testNr = 2;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<LevelXml>());
            Assert.That(((LevelXml)ts.Tests[testNr].Systems[0]).Members.ChildrenOf, Is.EqualTo("aBc"));
        }

    }
}
