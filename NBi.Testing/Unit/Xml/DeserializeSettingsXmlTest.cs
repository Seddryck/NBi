using System.IO;
#region Using directives

using System.Xml.Serialization;
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
    public class DeserializeSettingsXmlTest
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

        protected TestSuiteXml DeserializeSample(string filename)
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(string.Format("NBi.Testing.Unit.Xml.Resources.{0}.xml", filename)))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithDefault_DefaultLoaded()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsWithDefault");

            Assert.That(ts.Settings.Defaults.Count, Is.EqualTo(1));
            Assert.That(ts.Settings.Defaults[0].ApplyTo, Is.EqualTo(NBi.Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest));
            Assert.That(ts.Settings.Defaults[0].ConnectionString, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithDefault_DefaultReplicatedForTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsWithDefault");

            Assert.That(((QueryXml)ts.Tests[testNr].Systems[0]).ConnectionString, Is.Null.Or.Empty);
            Assert.That(((QueryXml)ts.Tests[testNr].Systems[0]).GetConnectionString(), Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithoutDefault_NoDefaultLoaded()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsWithoutDefault");

            Assert.That(ts.Settings.Defaults.Count, Is.EqualTo(0));
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithoutDefault_DefaultReplicatedForTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsWithoutDefault");

            Assert.That(((QueryXml)ts.Tests[testNr].Systems[0]).ConnectionString, Is.Null.Or.Empty);
        }

    }
}
