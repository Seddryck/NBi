using NBi.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core
{
    [TestFixture]
    public class CsvWriterTest
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

        private CsvProfile Csv = CsvProfile.SemiColumnDoubleQuote;

        [Test]
        public void Write_OneRowWithEuroSymbol_CorrectlyWritten()
        {
            var table = new DataTable();
            Load(table, new string[] { "symbol € EUR" }, "alpha1");

            var csvWriter = new CsvWriter(false);
            csvWriter.Write(table, "test.csv");

            using (Stream stream = new FileStream("test.csv", FileMode.Open))
            {
                stream.Position = 0;
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    var text = streamReader.ReadToEnd();
                    var firstCell = text.Split(new string[] { Csv.RecordSeparator }, StringSplitOptions.RemoveEmptyEntries)[0];
                    Assert.That(firstCell, Is.StringContaining("€"));
                }
            }
        }
    }
}
