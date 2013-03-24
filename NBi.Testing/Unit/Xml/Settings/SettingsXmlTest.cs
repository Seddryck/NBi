using System.IO;
#region Using directives

using NBi.Xml;
using System.Reflection;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml.Settings
{
    [TestFixture]
    public class SettingsXmlTest
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
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithDefault");

            Assert.That(ts.Settings.Defaults.Count, Is.EqualTo(1));
            Assert.That(ts.Settings.Defaults[testNr].ApplyTo, Is.EqualTo(NBi.Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest));
            Assert.That(ts.Settings.Defaults[testNr].ConnectionString, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithDefault_DefaultReplicatedForTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithDefault");

            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.ConnectionString, Is.Null.Or.Empty);
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithDefaultForAssert_DefaultReplicatedForTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithDefaultAssert");

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).GetCommand().Connection.ConnectionString, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithoutDefault_NoDefaultLoaded()
        {
            //int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithoutDefault");

            Assert.That(ts.Settings.Defaults.Count, Is.EqualTo(0));
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithoutDefault_DefaultReplicatedForTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithoutDefault");

            Assert.That(((QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).Item).ConnectionString, Is.Null.Or.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithoutDefault_DefaultEqualToNull()
        {
            //int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithoutDefault");

            Assert.That(ts.Settings.GetDefault(NBi.Xml.Settings.SettingsXml.DefaultScope.SystemUnderTest), Is.Null);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithReference_ReferenceLoaded()
        {
            //int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithReference");

            Assert.That(ts.Settings.References.Count, Is.EqualTo(2));
            Assert.That(ts.Settings.GetReference("first-ref"), Is.Not.Null);
            Assert.That(ts.Settings.GetReference("first-ref").ConnectionString, Is.EqualTo("My First Connection String"));
            Assert.That(ts.Settings.GetReference("second-ref"), Is.Not.Null);
            Assert.That(ts.Settings.GetReference("second-ref").ConnectionString, Is.EqualTo("My Second Connection String"));
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithReferenceNotExisting_ThrowArgumentException()
        {
            //int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithReference");

            Assert.Throws<System.ArgumentOutOfRangeException>(delegate { ts.Settings.GetReference("not-existing"); });
        }

        [Test]
        [Ignore ("Reference Not fully implemented")]
        public void DeserializeEqualToResultSet_SettingsWithReference_ReferenceAppliedToTest()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithReference");

            Assert.That(((QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).Item).ConnectionString, Is.Null.Or.Empty);
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToResultSet_SettingsWithReferenceButMissingconnStrRef_NullAppliedToTest()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("SettingsXmlWithReference");
            Assert.That(((QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).Item).ConnectionString, Is.Null.Or.Empty);
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Item.GetConnectionString(), Is.Null);
        }

    }
}
