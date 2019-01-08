using System.IO;
#region Using directives

using NBi.Xml;
using System.Reflection;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Xml.Settings;
using System;
#endregion

namespace NBi.Testing.Unit.Xml.Settings
{
    [TestFixture]
    public class CsvProfileXmlTest
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
            manager.ApplyDefaultSettings();
            return manager.TestSuite;
        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToTabCardinalLineFeed_True()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.InternalFieldSeparator, Is.EqualTo("Tab"));
            Assert.That(profile.FieldSeparator, Is.EqualTo('\t'));
            Assert.That(profile.InternalRecordSeparator, Is.EqualTo("#Lf"));
            Assert.That(profile.RecordSeparator, Is.EqualTo("#\n"));
        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToDefaultFirstRowHeader_False()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.FirstRowHeader, Is.False);
        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToDefaultMissingemptyValues_Default()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.EmptyCell, Is.EqualTo("(empty)"));
            Assert.That(profile.MissingCell, Is.EqualTo("(null)"));
        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToFirstRowHeader_True()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite2");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.FirstRowHeader, Is.True);
        }

        [Test]
        public void DeserializeCsvProfile_CsvProfileSetToEmptyCell_True()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample("CsvProfileXmlTestSuite2");

            //The Csv Profile is correctly set
            var profile = ts.Settings.CsvProfile;
            Assert.That(profile, Is.Not.Null);
            Assert.That(profile.EmptyCell, Is.EqualTo("empty value"));
            Assert.That(profile.MissingCell, Is.EqualTo("missing value"));
        }

        [Test]
        public void Serialize_CardinalForFieldSeparator_FieldSeparatorSpecified()
        {
            var profile = new CsvProfileXml
            {
                FieldSeparator = '#',
                RecordSeparator = "\r"
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CsvProfileXml>(profile);

            Assert.That(xml, Is.StringContaining("field-separator"));
            Assert.That(xml, Is.StringContaining("#"));

            Assert.That(xml, Is.StringContaining("record-separator"));
            Assert.That(xml, Is.StringContaining("Cr"));

            Assert.That(xml, Is.Not.StringContaining("<FieldSeparator>"));
            Assert.That(xml, Is.Not.StringContaining("<TextQualifier>"));
            Assert.That(xml, Is.Not.StringContaining("<RecordSeparator>"));
            Assert.That(xml, Is.Not.StringContaining("<FirstRowHeader>"));

            Assert.That(xml, Is.Not.StringContaining("first-row-header"));
        }

        [Test]
        public void Serialize_CrLfForRecordSeparator_RecordSeparatorNotSpecified()
        {
            var profile = new CsvProfileXml
            {
                FieldSeparator = '#',
                RecordSeparator = "\r\n"
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(profile);

            Assert.That(xml, Is.Not.StringContaining("record-separator"));
        }

        [Test]
        public void Serialize_TrueForFirstRowHeader_FirstRowHeaderSpecified()
        {
            var profile = new CsvProfileXml
            {
                FirstRowHeader = true
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(profile);

            Assert.That(xml, Is.StringContaining("first-row-header"));
        }

        [Test]
        public void Serialize_FalseForFirstRowHeader_FirstRowHeaderSpecified()
        {
            var profile = new CsvProfileXml { FirstRowHeader = false };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(profile);

            Assert.That(xml, Is.Not.StringContaining("first-row-header"));
        }

        [Test]
        public void Serialize_EmptyForEmpytCell_EmptyCellNotSpecified()
        {
            var profile = new CsvProfileXml { EmptyCell = "(empty)" };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(profile);

            Assert.That(xml, Is.Not.StringContaining("empty-cell"));
        }

        [Test]
        public void Serialize_StringForEmpytCell_EmptyCellSpecified()
        {
            var profile = new CsvProfileXml { EmptyCell = "my value" };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(profile);

            Assert.That(xml, Is.StringContaining("empty-cell"));
            Assert.That(xml, Is.StringContaining("my value"));
            Assert.That(xml, Is.Not.StringContaining("<EmptyCell"));
        }

        [Test]
        public void Serialize_SemiColumnForFieldSeparator_FieldSeparatorNotSpecified()
        {
            var profile = new CsvProfileXml
            {
                FieldSeparator = ';',
                RecordSeparator = "#"
            };

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(profile);

            Assert.That(xml, Is.Not.StringContaining("field-separator"));
        }

        [Test]
        public void Serialize_SemiColumnAndCrLf_CsvProfileNotSpecified()
        {
            var settings = new SettingsXml();
            settings.CsvProfile.FieldSeparator = ';';
            settings.CsvProfile.RecordSeparator = "\r\n";

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom(settings);

            Assert.That(xml, Is.Not.StringContaining("field-separator"));
            Assert.That(xml, Is.Not.StringContaining("record-separator"));
            Assert.That(xml, Is.Not.StringContaining("csv-profile"));
        }
    }
}
