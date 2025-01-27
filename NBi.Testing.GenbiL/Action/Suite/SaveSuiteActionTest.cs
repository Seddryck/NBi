using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Stateful;
using NBi.Xml;
using NUnit.Framework;
using System;
using System.IO;

namespace NBi.GenbiL.Testing.Action.Suite;

[TestFixture]
public class SaveSuiteActionTest
{
    protected TestSuiteXml DeserializeFile(string filename)
    {
        // Declare an object variable of the type to be deserialized.
        var manager = new XmlManager();

        // A Stream is needed to read the XML document.
        using (Stream stream = new FileStream(filename, FileMode.Open))
        using (var reader = new StreamReader(stream))
        {
            manager.Read(reader);
        }
        manager.ApplyDefaultSettings();
        return manager.TestSuite;
    }

    [Test]
    [TestCase("(empty)", "(null)", ';', "\r\n", '"', false)]    // All defaults
    [TestCase("(empty)", "(null)", ';', "\r\n", '"', true)]     // All defaults except FirstRowHeader
    [TestCase("#", "(null)", ';', "\r\n", '"', true)]           // All defaults except EmptyCell and FirstRowHeader
    [TestCase("(empty)", "#", ';', "\r\n", '"', true)]          // All defaults except MissingCell and FirstRowHeader
    [TestCase("(empty)", "(null)", '#', "\r\n", '"', true)]     // All defaults except FieldSeparator and FirstRowHeader
    [TestCase("(empty)", "(null)", ';', "#", '"', true)]        // All defaults except RecordSeparator and FirstRowHeader
    [TestCase("(empty)", "(null)", ';', "\r\n", '#', true)]     // All defaults except TextQualifier and FirstRowHeader

    [TestCase("#", "(null)", ';', "\r\n", '"', false)]           // All defaults except EmptyCell
    [TestCase("#", "(null)", '|', "\r\n", '"', false)]           // All defaults except EmptyCell and FieldSeparator

    [TestCase("(empty)", "#", ';', "\r\n", '"', false)]          // All defaults except MissingCell
    [TestCase("(empty)", "#", '|', "\r\n", '"', false)]          // All defaults except MissingCell and FieldSeparator
    public void Execute_CsvProfile_PropertiesSerialized(string emptyCell, string missingCell, char fieldSeparator, string recordSeparator, char textQualifier, bool firstRowHeader)
    {
        string filepath = Path.Combine(Path.GetTempPath(), $"Execute_CsvProfile_Saved_{ Guid.NewGuid() }.nbits");

        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }

        var state = new GenerationState();
        state.Settings.CsvProfile.EmptyCell = emptyCell;
        state.Settings.CsvProfile.MissingCell = missingCell;
        state.Settings.CsvProfile.FieldSeparator = fieldSeparator;
        state.Settings.CsvProfile.RecordSeparator = recordSeparator;
        state.Settings.CsvProfile.TextQualifier = textQualifier;
        state.Settings.CsvProfile.FirstRowHeader = firstRowHeader;

        var saveAction = new SaveSuiteAction(filepath);
        saveAction.Execute(state);

        var fileContent = DeserializeFile(filepath);
        Assert.That(fileContent.Settings.CsvProfile.EmptyCell, Is.EqualTo(emptyCell));
        Assert.That(fileContent.Settings.CsvProfile.MissingCell, Is.EqualTo(missingCell));
        Assert.That(fileContent.Settings.CsvProfile.FieldSeparator, Is.EqualTo(fieldSeparator));
        Assert.That(fileContent.Settings.CsvProfile.RecordSeparator, Is.EqualTo(recordSeparator));
        Assert.That(fileContent.Settings.CsvProfile.TextQualifier, Is.EqualTo(textQualifier));
        Assert.That(fileContent.Settings.CsvProfile.FirstRowHeader, Is.EqualTo(firstRowHeader));

        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
    }
}
