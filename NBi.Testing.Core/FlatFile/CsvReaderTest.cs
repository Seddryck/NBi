using NBi.Core.FlatFile;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.FlatFile;

[TestFixture]
//[Ignore("PocketCsvReader not compatible with .NET CORE?")]
public class CsvReaderTest
{
    class CsvReaderProxy : CsvReader
    {
        public CsvReaderProxy()
            : base(new CsvProfile(false)) { }
        public CsvReaderProxy(CsvProfile profile)
            : base(profile) { }
        public CsvReaderProxy(CsvProfile profile, int bufferSize)
            : base(profile, bufferSize) { }
        public CsvReaderProxy(int bufferSize)
            : base(CsvProfile.SemiColumnDoubleQuote, bufferSize) { }

        public new string? RemoveTextQualifier(string item, char textQualifier, char escapeTextQualifier)
            => base.RemoveTextQualifier(item, textQualifier, escapeTextQualifier);
        public new IEnumerable<string?> SplitLine(string row, char fieldSeparator, char textQualifier, char escapeTextQualifier, string emptyCell)
            => base.SplitLine(row, fieldSeparator, textQualifier, escapeTextQualifier, emptyCell);
        public new string GetFirstRecord(StreamReader reader, string recordSeparator, int bufferSize)
            => base.GetFirstRecord(reader, recordSeparator, bufferSize);
        public new int IdentifyPartialRecordSeparator(string text, string recordSeparator)
            => base.IdentifyPartialRecordSeparator(text, recordSeparator);
        public new string CleanRecord(string record, string recordSeparator)
            => base.CleanRecord(record, recordSeparator);
        public new DataTable Read(Stream stream)
            => base.Read(stream);
        public new DataTable Read(Stream stream, Encoding encoding, int encodingBytesCount, bool isFirstRowHeader, string recordSeparator, char fieldSeparator, char textQualifier, char escapeTextQualifier, char commentChar, string emptyCell, string missingCell)
            => base.Read(stream, encoding, encodingBytesCount, isFirstRowHeader, recordSeparator, fieldSeparator, textQualifier, escapeTextQualifier, commentChar, emptyCell, missingCell);
    }

    [Test]
    [TestCase(null, "")]
    [TestCase("(null)", null)] //Parse (null) to a real null value
    [TestCase("\"(null)\"", "(null)")] //Explicitly quoted (null) should be (null)
    [TestCase("null", "null")]
    [TestCase("", "")]
    [TestCase("a", "a")]
    [TestCase("\"", "\"")]
    [TestCase("\"a", "\"a")]
    [TestCase("ab", "ab")]
    [TestCase("\"ab\"", "ab")]
    [TestCase("abc", "abc")]
    [TestCase("\"abc\"", "abc")]
    [TestCase("\"a\"\"b\"", "a\"b")]
    [TestCase("\"\"\"a\"\"b\"\"\"", "\"a\"b\"")]
    public void RemoveTextQualifier_String_CorrectString(string item, string result)
    {
        var reader = new CsvReaderProxy();
        var value = reader.RemoveTextQualifier(item, '\"', '\"');
        Assert.That(value, Is.EqualTo(result));
    }

    [Test]
    public void SplitLine_Null_NotEmpty()
    {
        var reader = new CsvReaderProxy();
        var values = reader.SplitLine("a;(null)", ';', char.MinValue, char.MinValue, string.Empty);
        Assert.That(values.ElementAt(1), Is.Null);
    }

    [Test]
    [TestCase("abc+abc+abc+abc", "+", 1)]
    [TestCase("abc+abc+abc+abc", "+", 2)]
    [TestCase("abc+abc+abc+abc", "+", 200)]
    [TestCase("abc+@abc+@abc+@abc", "+@", 1)]
    [TestCase("abc+@abc+@abc+@abc", "+@", 2)]
    [TestCase("abc+@abc+@abc+@abc", "+@", 4)]
    [TestCase("abc+@abc+@abc+@abc", "+@", 5)]
    [TestCase("abc+@abc+@abc+@abc", "+@", 200)]
    [TestCase("abc+@abc+abc+@abc", "+@", 1)]
    [TestCase("abc+@abc+abc+@abc", "+@", 2)]
    [TestCase("abc+@abc+abc+@abc", "+@", 4)]
    [TestCase("abc+@abc+abc+@abc", "+@", 5)]
    [TestCase("abc+@abc+abc+@abc", "+@", 200)]
    [TestCase("abc+@abc+abc+@abc+@", "+@", 1)]
    [TestCase("abc+@abc+abc+@abc+@", "+@", 2)]
    [TestCase("abc+@abc+abc+@abc+@", "+@", 4)]
    [TestCase("abc+@abc+abc+@abc+@", "+@", 5)]
    [TestCase("abc+@abc+abc+@abc+@", "+@", 200)]
    [TestCase("abc", "+@", 200)]
    public void GetFirstRecord_Csv_CorrectResult(string text, string recordSeparator, int bufferSize)
    {
        using var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(text);
        writer.Flush();

        stream.Position = 0;

        var reader = new CsvReaderProxy();
        using (var streamReader = new StreamReader(stream, Encoding.UTF8, true))
        {
            var value = reader.GetFirstRecord(streamReader, recordSeparator, bufferSize);
            Assert.That(value, Is.EqualTo("abc" + recordSeparator).Or.EqualTo("abc"));
        }
        writer.Dispose();
    }

    [Test]
    [TestCase("abc+abc++abc+abc", "++", 1)]
    public void GetFirstRecord_CsvWithSemiSeparator_CorrectResult(string text, string recordSeparator, int bufferSize)
    {
        using var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(text);
        writer.Flush();

        stream.Position = 0;

        var reader = new CsvReaderProxy();
        using (var streamReader = new StreamReader(stream, Encoding.UTF8, true))
        {
            var value = reader.GetFirstRecord(streamReader, recordSeparator, bufferSize);
            Assert.That(value, Is.EqualTo("abc+abc" + recordSeparator).Or.EqualTo("abc+abc"));
        }
        writer.Dispose();
    }

    [Test]
    [TestCase("a+b+c#a+b#a#a+b", '+', "#", "?")]
    public void Read_CsvWithCsvProfileMissingCell_CorrectResults(string text, char fieldSeparator, string recordSeparator, string missingCell)
    {
        using var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(text);
        writer.Flush();

        stream.Position = 0;
        var reader = new CsvReaderProxy();
        var dataTable = reader.Read(stream, Encoding.UTF8, 0, false, recordSeparator, fieldSeparator, '\"', '\"', '!', "_", missingCell);

        Assert.That(dataTable.Rows[0][0], Is.EqualTo("a"));
        Assert.That(dataTable.Rows[0][1], Is.EqualTo("b"));
        Assert.That(dataTable.Rows[0][2], Is.EqualTo("c"));

        Assert.That(dataTable.Rows[1][0], Is.EqualTo("a"));
        Assert.That(dataTable.Rows[1][1], Is.EqualTo("b"));
        Assert.That(dataTable.Rows[1][2], Is.EqualTo("?"));

        Assert.That(dataTable.Rows[2][0], Is.EqualTo("a"));
        Assert.That(dataTable.Rows[2][1], Is.EqualTo("?"));
        Assert.That(dataTable.Rows[2][2], Is.EqualTo("?"));

        Assert.That(dataTable.Rows[3][0], Is.EqualTo("a"));
        Assert.That(dataTable.Rows[3][1], Is.EqualTo("b"));
        Assert.That(dataTable.Rows[3][2], Is.EqualTo("?"));

        writer.Dispose();
    }

    [Test]
    [TestCase("a+b+c#a++c#+b+c#+b+", '+', "#", "?")]
    public void Read_CsvWithCsvProfileEmptyCell_CorrectResults(string text, char fieldSeparator, string recordSeparator, string emptyCell)
    {
        using var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(text);
        writer.Flush();

        stream.Position = 0;
        var reader = new CsvReaderProxy();
        var dataTable = reader.Read(stream, Encoding.UTF8, 0, false, recordSeparator, fieldSeparator, '\"', '\"', '!', emptyCell, "_");

        Assert.That(dataTable.Rows[0][0], Is.EqualTo("a"));
        Assert.That(dataTable.Rows[0][1], Is.EqualTo("b"));
        Assert.That(dataTable.Rows[0][2], Is.EqualTo("c"));

        Assert.That(dataTable.Rows[1][0], Is.EqualTo("a"));
        Assert.That(dataTable.Rows[1][1], Is.EqualTo("?"));
        Assert.That(dataTable.Rows[1][2], Is.EqualTo("c"));

        Assert.That(dataTable.Rows[2][0], Is.EqualTo("?"));
        Assert.That(dataTable.Rows[2][1], Is.EqualTo("b"));
        Assert.That(dataTable.Rows[2][2], Is.EqualTo("c"));

        Assert.That(dataTable.Rows[3][0], Is.EqualTo("?"));
        Assert.That(dataTable.Rows[3][1], Is.EqualTo("b"));
        Assert.That(dataTable.Rows[3][2], Is.EqualTo("?"));

        writer.Dispose();
    }

    [Test]
    [TestCase("abc\r\ndef\r\nghl\r\nijk", 1, 1)]
    [TestCase("abc\r\ndef\r\nghl\r\nijk", 17, 1)]
    [TestCase("abc\r\ndef\r\nghl\r\nijk", 18, 1)]
    [TestCase("abc\r\ndef\r\nghl\r\nijk", 19, 1)]
    [TestCase("abc\r\ndef\r\nghl\r\nijk", 512, 1)]
    [TestCase("abc;xyz\r\ndef;xyz\r\nghl\r\n;ijk", 1, 2)]
    [TestCase("abc;xyz\r\ndef;xyz\r\nghl\r\n;ijk", 512, 2)]
    public void Read_Csv_CorrectResult(string text, int bufferSize, int columnCount)
    {
        using var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(text);
        writer.Flush();

        stream.Position = 0;

        var reader = new CsvReaderProxy(new CsvProfile(';', '\"', "\r\n", false, false, "(empty)", "(null)"), bufferSize);
        var dataTable = reader.Read(stream);
        Assert.That(dataTable.Rows, Has.Count.EqualTo(4));
        Assert.That(dataTable.Columns, Has.Count.EqualTo(columnCount));
        foreach (DataRow row in dataTable.Rows)
        {
            foreach (var cell in row.ItemArray)
                Assert.That(cell?.ToString(), Has.Length.EqualTo(3).Or.EqualTo("(empty)").Or.EqualTo("(null)"));
        }
        writer.Dispose();
    }

    [Test]
    [TestCase("abc", "123", 0)]
    [TestCase("abc1", "123", 1)]
    [TestCase("abc12", "123", 2)]
    [TestCase("abc12a", "123", 0)]
    [TestCase("", "123", 0)]
    [TestCase("", "#", 0)]
    [TestCase("abc", "#", 0)]
    public void IdentifyPartialRecordSeparator_Csv_CorrectResult(string text, string recordSeparator, int result)
    {
        var reader = new CsvReaderProxy(20);
        var value = reader.IdentifyPartialRecordSeparator(text, recordSeparator);
        Assert.That(value, Is.EqualTo(result));
    }

    [Test]
    [TestCase("a;b;c\r\nd;e;f;g\r\n", 1, 1)]
    [TestCase("a;b;c\r\nd;e;f\r\ng;h;i;j\r\n", 2, 1)]
    [TestCase("a;b;c\r\nd;e;f\r\ng;h;i;j;k\r\n", 2, 2)]
    public void Read_MoreFieldThanExpected_ExceptionMessage(string text, int rowNumber, int moreField)
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        writer.Write(text);
        writer.Flush();

        stream.Position = 0;

        var profile = CsvProfile.SemiColumnDoubleQuote;
        var reader = new CsvReaderProxy(profile);

        var ex = Assert.Throws<InvalidDataException>(delegate { reader.Read(stream); });
        Assert.That(ex!.Message, Does.Contain(string.Format("record {0} ", rowNumber + 1)));
        Assert.That(ex.Message, Does.Contain(string.Format("{0} more", moreField)));
    }

    [Test]
    public void Read_EmptyValue_MatchWithEmpty()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        writer.Write("a;;c");
        writer.Flush();

        stream.Position = 0;

        var profile = CsvProfile.SemiColumnDoubleQuote;
        var reader = new CsvReaderProxy(profile);
        var dataTable = reader.Read(stream);
        Assert.That(dataTable.Rows[0][1], Is.EqualTo(string.Empty).Or.EqualTo("(empty)"));
    }

    [Test]
    public void Read_MissingValue_MatchWithNullValue()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        writer.Write("a;b;c\r\na;b\r\na;b;c");
        writer.Flush();

        stream.Position = 0;

        var profile = CsvProfile.SemiColumnDoubleQuote;
        var reader = new CsvReaderProxy(profile);
        var dataTable = reader.Read(stream);
        Assert.That(dataTable.Rows[1][2], Is.EqualTo("(null)"));
    }
}
