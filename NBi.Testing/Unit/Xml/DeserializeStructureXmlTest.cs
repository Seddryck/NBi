#region Using directives
using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Systems.Structure;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class DeserializeStructureXmlTest
    {


        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.TestSuiteStructure.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_Hierarchy()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<HierarchyXml>());
            Assert.That(((HierarchyXml)ts.Tests[testNr].Systems[0]).Caption, Is.EqualTo("hierarchy"));
            Assert.That(((HierarchyXml)ts.Tests[testNr].Systems[0]).Dimension, Is.EqualTo("dimension"));
            Assert.That(((HierarchyXml)ts.Tests[testNr].Systems[0]).Perspective, Is.EqualTo("Perspective"));
            Assert.That(((HierarchyXml)ts.Tests[testNr].Systems[0]).ConnectionString, Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Deserialize_SampleFile_MeasureGroup()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MeasureGroupXml>());
            Assert.That(((MeasureGroupXml)ts.Tests[testNr].Systems[0]).Perspective, Is.EqualTo("Perspective"));
            Assert.That(((MeasureGroupXml)ts.Tests[testNr].Systems[0]).ConnectionString, Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Deserialize_SampleFile_ContainsConstraintWithoutDisplayFolder()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Specification.IsDisplayFolderSpecified, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ContainsConstraintWithDisplayFolder()
        {
            int testNr = 2;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).DisplayFolder, Is.EqualTo("aBc"));
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Specification.IsDisplayFolderSpecified, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_ContainsConstraintWithDisplayFolderRoot()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).DisplayFolder, Is.EqualTo(""));
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Specification.IsDisplayFolderSpecified, Is.True);
        }
                

    }
}
