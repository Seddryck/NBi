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
public class CsvReaderTest
{
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
        var reader = new CsvReader(profile);

        var ex = Assert.Throws<InvalidDataException>(delegate { reader.ToDataTable(stream); });
        Assert.That(ex!.Message, Does.Contain($"record {rowNumber + 1} "));
        Assert.That(ex.Message, Does.Contain($"{moreField} more"));
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
        var reader = new CsvReader(profile);
        var dataTable = reader.ToDataTable(stream);
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
        var reader = new CsvReader(profile);
        var dataTable = reader.ToDataTable(stream);
        Assert.That(dataTable.Rows[1][2], Is.EqualTo("(null)"));
    }
}
