using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Systems
{
    [TestFixture]
    public class MembersXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.MembersXmlTestSuite.xml"))
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
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MembersXml>());
            Assert.That(((MembersXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<LevelXml>());

            LevelXml item = (LevelXml)((MembersXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Dimension, Is.EqualTo("dimension"));
            Assert.That(item.Hierarchy, Is.EqualTo("hierarchy"));
            Assert.That(item.Caption, Is.EqualTo("level"));
            Assert.That(item.Perspective, Is.EqualTo("Perspective"));
            Assert.That(item.GetConnectionString(), Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Deserialize_SampleFile_AutoCategories()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var autoCategories = ts.Tests[testNr].Systems[0].GetAutoCategories();

            Assert.That(autoCategories, Has.Member("Dimension 'dimension'"));
            Assert.That(autoCategories, Has.Member("Hierarchy 'hierarchy'"));
            Assert.That(autoCategories, Has.Member("Perspective 'Perspective'"));
            Assert.That(autoCategories, Has.Member("Members"));
        }

        [Test]
        public void Deserialize_SampleFile_MembersWithHierarchy()
        {
            int testNr = 1;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MembersXml>());
            Assert.That(((MembersXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchyXml>());

            HierarchyXml item = (HierarchyXml)((MembersXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Dimension, Is.EqualTo("dimension"));
            Assert.That(item.Caption, Is.EqualTo("hierarchy"));
            Assert.That(item.Perspective, Is.EqualTo("Perspective"));
            Assert.That(item.GetConnectionString(), Is.EqualTo("ConnectionString"));
        }
        
        [Test]
        public void Deserialize_SampleFile_MembersWithChildrenOf()
        {
            int testNr = 2;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MembersXml>());
            Assert.That(((MembersXml)ts.Tests[testNr].Systems[0]).ChildrenOf, Is.EqualTo("aBc"));
        }

    }
}
