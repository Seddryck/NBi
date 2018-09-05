#region Using directives
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
using NBi.Xml.Constraints.Comparer;
using NBi.Core.ResultSet;
using NBi.Xml.Settings;
using NBi.Core.Evaluate;
using NBi.Xml.Items.Calculation;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
#endregion

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class ScoreXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.ScoreXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyScore()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ScoreXml>());
            Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_DefaultThersholdIsOne()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ScoreXml>());
            Assert.That((ts.Tests[testNr].Constraints[0] as ScoreXml).Threshold, Is.EqualTo(1));
        }

        [Test]
        public void Deserialize_SampleFile_DefaultThersholdIsValueAssigned()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ScoreXml>());
            Assert.That((ts.Tests[testNr].Constraints[0] as ScoreXml).Threshold, Is.EqualTo(0.95));
        }
    }
}
