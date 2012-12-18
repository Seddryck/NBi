#region Using directives
using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml.Systems
{
    [TestFixture]
    public class StructureXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.StructureXmlTestSuite.xml"))
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
            
            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchyXml>());

            HierarchyXml item = (HierarchyXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Caption, Is.EqualTo("hierarchy"));
            Assert.That(item.Dimension, Is.EqualTo("dimension"));
            Assert.That(item.Perspective, Is.EqualTo("Perspective"));
            Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Deserialize_SampleFile_MeasureGroup()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<MeasureGroupXml>());

            MeasureGroupXml item = (MeasureGroupXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Perspective, Is.EqualTo("Perspective"));
            Assert.That(item.ConnectionString, Is.EqualTo("ConnectionString"));
        }

       
        

    }
}
