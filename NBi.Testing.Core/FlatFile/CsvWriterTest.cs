using NBi.Core;
using NBi.Core.FlatFile;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.FlatFile
{
    class CsvWriterTest
    {
        private void Load(DataTable table, string[] rows, string columnNames)
        {
            var columns = columnNames.Split(',');
            for (int i = 0; i < columns.Length; i++)
                table.Columns.Add(new DataColumn(columns[i]));

            foreach (var row in rows)
            {
                var newRow = table.NewRow();
                newRow.ItemArray = row.Split(',');
                table.Rows.Add(newRow);
            }

            table.AcceptChanges();
        }

        private readonly CsvProfile Csv = CsvProfile.SemiColumnDoubleQuote;

        [Test]
        public void Write_TwoRowsWithHeader_ThreeLines()
        {
            var table = new DataTable();
            Load(table, ["a11,a12", "a21,a22"], "alpha1,alpha2");

            var csvWriter = new CsvWriter(true);
            using MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(stream);
            csvWriter.Write(table, streamWriter);

            stream.Position = 0;
            using var streamReader = new StreamReader(stream);
            var text = streamReader.ReadToEnd();
            text.Remove(text.Length - 2); //Avoid miscount if last line as a record separator or not
            var countLine = text.Count(c => c == Csv.Descriptor.LineTerminator[0]);
            Assert.That(countLine, Is.EqualTo(3));
        }

        [Test]
        public void Write_TwoRowsWithoutHeader_TwoLines()
        {
            var table = new DataTable();
            Load(table, ["a11,a12", "a21,a22"], "alpha1,alpha2");

            var csvWriter = new CsvWriter(false);
            using MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(stream);
            csvWriter.Write(table, streamWriter);

            stream.Position = 0;
            using StreamReader streamReader = new StreamReader(stream);
            var text = streamReader.ReadToEnd();
            text.Remove(text.Length - 2); //Avoid miscount if last line as a record separator or not
            var countLine = text.Count(c => c == Csv.Descriptor.LineTerminator[0]);
            Assert.That(countLine, Is.EqualTo(2));
        }

        [Test]
        public void Write_TwoRowsWithoutHeader_AllLinesHaveTwoFieldSeparator()
        {
            var table = new DataTable();
            Load(table, ["a11,a12,a13", "a21,a22,a23"], "alpha1,alpha2,alpha3");

            var csvWriter = new CsvWriter(false);
            using MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(stream);
            csvWriter.Write(table, streamWriter);

            stream.Position = 0;
            using StreamReader streamReader = new StreamReader(stream);
            var text = streamReader.ReadToEnd();
            var lines = text.Split(new string[] { Csv.Descriptor.LineTerminator }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var countLine = line.Count(c => c == Csv.Descriptor.Delimiter);
                Assert.That(countLine, Is.EqualTo(2));
            }
        }

        [Test]
        public void Write_TwoRowsWithHeader_HeaderIsCorrect()
        {
            var table = new DataTable();
            var columnNames = "alpha1,alpha2,alpha3";
            Load(table, ["a11,a12,a13", "a21,a22,a23"], columnNames);

            var csvWriter = new CsvWriter(true);
            using MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(stream);
            csvWriter.Write(table, streamWriter);

            stream.Position = 0;
            using StreamReader streamReader = new StreamReader(stream);
            var text = streamReader.ReadToEnd();
            var lines = text.Split(new string[] { Csv.Descriptor.LineTerminator }, StringSplitOptions.RemoveEmptyEntries);
            var lineHeader = lines[0];
            var fields = lineHeader.Split(Csv.Descriptor.Delimiter);
            Assert.That(fields, Is.EqualTo(columnNames.Split(',')));
        }

        [Test]
        public void Write_OneRowNeedQuoting_CorrectlyQuoted()
        {
            var table = new DataTable();
            Load(table, ["a;11"], "alpha1");

            var csvWriter = new CsvWriter(false);
            using MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(stream);
            csvWriter.Write(table, streamWriter);

            stream.Position = 0;
            using StreamReader streamReader = new StreamReader(stream);
            var text = streamReader.ReadToEnd();
            var firstCell = text.Split(new string[] { Csv.Descriptor.LineTerminator }, StringSplitOptions.RemoveEmptyEntries)[0];
            Assert.That(firstCell, Does.StartWith(Csv.Descriptor.QuoteChar.ToString()));
            Assert.That(firstCell, Does.EndWith(Csv.Descriptor.QuoteChar.ToString()));
            Assert.That(firstCell, Does.Contain(Csv.Descriptor.Delimiter.ToString()));
        }

        [Test]
        public void Write_OneRowDontNeedQuoting_CorrectlyNotQuoted()
        {
            var table = new DataTable();
            Load(table, ["a11"], "alpha1");

            var csvWriter = new CsvWriter(false);
            using var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            csvWriter.Write(table, streamWriter);

            stream.Position = 0;
            using StreamReader streamReader = new StreamReader(stream);
            var text = streamReader.ReadToEnd();
            var firstCell = text.Split(new string[] { Csv.Descriptor.LineTerminator }, StringSplitOptions.RemoveEmptyEntries)[0];
            Assert.That(firstCell, Does.Not.StartsWith(Csv.Descriptor.QuoteChar.ToString()));
            Assert.That(firstCell, Does.Not.EndsWith(Csv.Descriptor.QuoteChar.ToString()));
            Assert.That(firstCell, Does.Not.Contain(Csv.Descriptor.Delimiter.ToString()));
        }
    }
}
