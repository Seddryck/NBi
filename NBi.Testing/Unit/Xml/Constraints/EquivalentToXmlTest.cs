#region Using directives
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class EquivalentToXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.EquivalentToXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void DeserializeEquivalentToItems_ListOfItems_Inline()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EquivalentToXml>());
            Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Items, Is.Not.Null);
            Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(2));
            Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Items[0], Is.EqualTo("Hello"));
            Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Items[1], Is.EqualTo("World"));
        }

        [Test]
        public void DeserializeEquivalentToOneColumnQuery_SqlQuery_Inline()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EquivalentToXml>());
            Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Query, Is.Not.Null);
            Assert.That(((EquivalentToXml)ts.Tests[testNr].Constraints[0]).Query.GetQuery(), Is.StringContaining("Hello").And.StringContaining("World"));
        }
    }
}
