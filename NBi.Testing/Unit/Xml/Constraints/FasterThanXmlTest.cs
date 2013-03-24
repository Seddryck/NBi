#region Using directives
using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class FasterThanXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.FasterThanXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyParametersFasterThanConstraint()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<FasterThanXml>());
            Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).CleanCache, Is.True);
            Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).MaxTimeMilliSeconds, Is.EqualTo(100));
        }

        [Test]
        public void Deserialize_SampleFile_AcceptIntMaxValueValueForMaxTimeMilliSeconds()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<FasterThanXml>());
            Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).MaxTimeMilliSeconds, Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void Deserialize_SampleFile_DefaultValueForCleanCacheIsFalse()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<FasterThanXml>());
            Assert.That(((FasterThanXml)ts.Tests[testNr].Constraints[0]).CleanCache, Is.False);
        }

        

    }
}
