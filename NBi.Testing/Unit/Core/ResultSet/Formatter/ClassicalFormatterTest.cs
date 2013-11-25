using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Core.ResultSet.Formatter;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Formatter
{
    class ClassicalFormatterTest
    {
        [Test]
        public void Tabulize_Empty_CorrectLength()
        {
            // Design Dummy Column
            DataColumn colKey = new DataColumn("BusinessKey");
            colKey.ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            colKey.ExtendedProperties.Add("NBi::Type", ColumnType.Text);

            // Design Dummy Column
            DataColumn colValue = new DataColumn("NumValue");
            colValue.ExtendedProperties.Add("NBi::Role", ColumnRole.Value);
            colValue.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            colValue.ExtendedProperties.Add("NBi::Tolerance", new NumericAbsoluteTolerance(new decimal(0.001)));

            // Design dummy table
            DataTable table = new DataTable();
            table.Columns.Add(colKey);
            table.Columns.Add(colValue);

            // Design dummy rows
            var row = table.NewRow();
            row[0]  = "Alpha";
            row[1] = 77.005;
            table.Rows.Add(row);
            row = table.NewRow();
            row[0] = "Beta";
            row[1] = 103.5;
            table.Rows.Add(row);

            var formatter = new ClassicalFormatter();
            var text = formatter.Tabulize(table.Rows.Cast<DataRow>());
            var lines = text.Replace("\n", "").Split('\r');

            //Remove non-tabular rows
            var tabularLines = lines.ToList();
            tabularLines.RemoveAt(0);
            tabularLines.RemoveAt(tabularLines.Count()-1);

            //Check that each row is starting and ending by a "|"
            foreach (var line in tabularLines)
            {
                Assert.That(line.StartsWith("|"));
                Assert.That(line.EndsWith("|"));
            }

            //Check that each row has the same length
            foreach (var line in tabularLines)
            {
                Assert.That(line.Length, Is.EqualTo(32));
            }

        }
    }
}
