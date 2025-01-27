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
using System.Collections.Generic;
#endregion

namespace NBi.Xml.Testing.Unit.Settings;

[TestFixture]
public class CsvProfileXmlTest
{

    #region SetUp & TearDown
    //Called only at instance creation
    [OneTimeSetUp]
    public void SetupMethods()
    {

    }

    //Called only at instance destruction
    [OneTimeTearDown]
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
        using (var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Resources.{filename}.xml")
                                       ?? throw new FileNotFoundException())
        using (var reader = new StreamReader(stream))
        {
            manager.Read(reader);
        }
        manager.ApplyDefaultSettings();
        return manager.TestSuite!;
    }

    [Test]
    public void DeserializeCsvProfile_CsvProfileSetToTabCardinalLineFeed_True()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("CsvProfileXmlTestSuite");

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
        var ts = DeserializeSample("CsvProfileXmlTestSuite");

        //The Csv Profile is correctly set
        var profile = ts.Settings.CsvProfile;
        Assert.That(profile, Is.Not.Null);
        Assert.That(profile.FirstRowHeader, Is.False);
    }

    [Test]
    public void DeserializeCsvProfile_CsvProfileSetToDefaultMissingemptyValues_Default()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("CsvProfileXmlTestSuite");

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
        var ts = DeserializeSample("CsvProfileXmlTestSuite2");

        //The Csv Profile is correctly set
        var profile = ts.Settings.CsvProfile;
        Assert.That(profile, Is.Not.Null);
        Assert.That(profile.FirstRowHeader, Is.True);
    }

    [Test]
    public void DeserializeCsvProfile_CsvProfileSetToEmptyCell_True()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample("CsvProfileXmlTestSuite2");

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

        Assert.That(xml, Does.Contain("field-separator"));
        Assert.That(xml, Does.Contain("#"));

        Assert.That(xml, Does.Contain("record-separator"));
        Assert.That(xml, Does.Contain("Cr"));

        Assert.That(xml, Does.Not.Contain("<FieldSeparator>"));
        Assert.That(xml, Does.Not.Contain("<TextQualifier>"));
        Assert.That(xml, Does.Not.Contain("<RecordSeparator>"));
        Assert.That(xml, Does.Not.Contain("<FirstRowHeader>"));

        Assert.That(xml, Does.Not.Contain("first-row-header"));
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

        Assert.That(xml, Does.Not.Contain("record-separator"));
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

        Assert.That(xml, Does.Contain("first-row-header"));
    }

    [Test]
    public void Serialize_FalseForFirstRowHeader_FirstRowHeaderSpecified()
    {
        var profile = new CsvProfileXml { FirstRowHeader = false };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(profile);

        Assert.That(xml, Does.Not.Contain("first-row-header"));
    }

    [Test]
    public void Serialize_EmptyForEmpytCell_EmptyCellNotSpecified()
    {
        var profile = new CsvProfileXml { EmptyCell = "(empty)" };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(profile);

        Assert.That(xml, Does.Not.Contain("empty-cell"));
    }

    [Test]
    public void Serialize_StringForEmpytCell_EmptyCellSpecified()
    {
        var profile = new CsvProfileXml { EmptyCell = "my value" };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(profile);

        Assert.That(xml, Does.Contain("empty-cell"));
        Assert.That(xml, Does.Contain("my value"));
        Assert.That(xml, Does.Not.Contain("<EmptyCell"));
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

        Assert.That(xml, Does.Not.Contain("field-separator"));
    }

    [Test]
    public void Serialize_SemiColumnAndCrLf_CsvProfileNotSpecified()
    {
        var settings = new SettingsXml();
        settings.CsvProfile.FieldSeparator = ';';
        settings.CsvProfile.RecordSeparator = "\r\n";

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(settings);

        Assert.That(xml, Does.Not.Contain("field-separator"));
        Assert.That(xml, Does.Not.Contain("record-separator"));
        Assert.That(xml, Does.Not.Contain("text-qualifier"));
        Assert.That(xml, Does.Not.Contain("csv-profile"));
    }

    [Test]
    public void Serialize_SingleQuote_CsvProfileSpecified()
    {
        var settings = new SettingsXml();
        settings.CsvProfile.TextQualifier = '\'';

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(settings);

        Assert.That(xml, Does.Not.Contain("field-separator"));
        Assert.That(xml, Does.Not.Contain("record-separator"));
        Assert.That(xml, Does.Contain("text-qualifier"));
        Assert.That(xml, Does.Contain("csv-profile"));
    }

    [Test]
    public void Serialize_DoubleQuote_CsvProfileNotSpecified()
    {
        var settings = new SettingsXml();
        settings.CsvProfile.TextQualifier = '\"';

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(settings);

        Assert.That(xml, Does.Not.Contain("field-separator"));
        Assert.That(xml, Does.Not.Contain("record-separator"));
        Assert.That(xml, Does.Not.Contain("text-qualifier"));
        Assert.That(xml, Does.Not.Contain("csv-profile"));
    }


    [Test]
    public void Attributes_Default_CorrectValue()
    {
        var settings = new SettingsXml();

        var expected = new Dictionary<string, object>()
            {
                { "field-separator", ';' },
                { "text-qualifier", '\"' },
                { "record-separator", "\r\n" },
                { "first-row-header", false },
                { "missing-cell", "(null)" },
                { "empty-cell", "(empty)" },
            };

        Assert.That(settings.CsvProfile.Attributes, Is.EqualTo(expected));
    }

    [Test]
    public void Attributes_Custom_CorrectValue()
    {
        var settings = new SettingsXml();
        settings.CsvProfile.EmptyCell = "(null)";
        settings.CsvProfile.TextQualifier = '\'';
        settings.CsvProfile.RecordSeparator = "\n";
        settings.CsvProfile.FirstRowHeader = true;

        var expected = new Dictionary<string, object>()
            {
                { "field-separator", ';' },
                { "text-qualifier", '\'' },
                { "record-separator", "\n" },
                { "first-row-header", true },
                { "missing-cell", "(null)" },
                { "empty-cell", "(null)" },
            };

        Assert.That(settings.CsvProfile.Attributes, Is.EqualTo(expected));
    }
}
