using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;

namespace NBi.Testing.GenbiL.Parser
{
    [TestFixture]
    public class CsvProfileParserTest
    {
        [Test]
        public void SentenceParser_FieldSeparatorAssert_ValidSentence()
        {
            var input = "csv-profile set field-separator to '#';";
            var result = CsvProfile.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CsvProfileFieldSeparatorAction>());
            Assert.That(((CsvProfileFieldSeparatorAction)result).Value, Is.EqualTo('#'));
        }

        [Test]
        public void SentenceParser_RecordSeparatorAssert_ValidSentence()
        {
            var input = "csv-profile set record-separator to 'Tab';";
            var result = CsvProfile.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CsvProfileRecordSeparatorAction>());
            Assert.That(((CsvProfileRecordSeparatorAction)result).Value, Is.EqualTo("Tab"));
        }

        [Test]
        public void SentenceParser_TextQualifierAssert_ValidSentence()
        {
            var input = "csv-profile set text-qualifier to '#';";
            var result = CsvProfile.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CsvProfileTextQualifierAction>());
            Assert.That(((CsvProfileTextQualifierAction)result).Value, Is.EqualTo('#'));
        }

        [Test]
        public void SentenceParser_FirstRowHeaderAssert_ValidSentence()
        {
            var input = "csv-profile set first-row-header to true;";
            var result = CsvProfile.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CsvProfileFirstRowHeaderAction>());
            Assert.That(((CsvProfileFirstRowHeaderAction)result).Value, Is.True);
        }

        [Test]
        public void SentenceParser_EmptyCellAssert_ValidSentence()
        {
            var input = "csv-profile set empty-cell to '...';";
            var result = CsvProfile.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CsvProfileEmptyCellAction>());
            Assert.That(((CsvProfileEmptyCellAction)result).Value, Is.EqualTo("..."));
        }

        [Test]
        public void SentenceParser_MissingCellAssert_ValidSentence()
        {
            var input = "csv-profile set missing-cell to '?';";
            var result = CsvProfile.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CsvProfileMissingCellAction>());
            Assert.That(((CsvProfileMissingCellAction)result).Value, Is.EqualTo("?"));
        }

    }
}
