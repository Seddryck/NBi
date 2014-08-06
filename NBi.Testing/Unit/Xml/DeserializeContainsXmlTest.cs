
#region Using directives

using System.IO;
using NBi.Xml;
using System.Reflection;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Core.ResultSet;

#endregion

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class DeserializeContainsXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.ContainsTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_ContainsWithSeveralItems_ItemsSetAndQueryNull()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            var ctr = (ContainsXml)ts.Tests[testNr].Constraints[0];

            Assert.That(ctr.ItemList.Count, Is.EqualTo(2));
            Assert.That(ctr.ItemList[0], Is.EqualTo("alpha"));
            Assert.That(ctr.ItemList[1], Is.EqualTo("beta"));
            Assert.That(ctr.Query, Is.Null);
        }

        [Test]
        public void Deserialize_ContainsWithOnlyQuery_QuerySetAndItemsEmpty()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            var ctr = (ContainsXml)ts.Tests[testNr].Constraints[0];

            Assert.That(ctr.ItemList.Count, Is.EqualTo(0));
            Assert.That(ctr.Query, Is.Not.Null);
        }

        [Test]
        public void Deserialize_ContainsWithSeveralItems_ItemsAndQuerySet()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            var ctr = (ContainsXml)ts.Tests[testNr].Constraints[0];

            Assert.That(ctr.ItemList.Count, Is.EqualTo(2));
            Assert.That(ctr.ItemList[0], Is.EqualTo("alpha"));
            Assert.That(ctr.ItemList[1], Is.EqualTo("beta"));
            Assert.That(ctr.Query, Is.Not.Null);
        }
        
    }
}
