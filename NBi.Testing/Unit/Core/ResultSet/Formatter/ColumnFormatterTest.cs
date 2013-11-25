using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using fmt = NBi.Core.ResultSet.Formatter;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Formatter
{
    [TestFixture]
    public class ColumnFormatterTest
    {
        [Test]
        public void Tabulize_Empty_CorrectLength()
        {
            // Design Dummy Column
            DataColumn colValue = new DataColumn("TextValue");
            colValue.ExtendedProperties.Add("NBi::Role", ColumnRole.Value);
            colValue.ExtendedProperties.Add("NBi::Type", ColumnType.Text);

            // Design dummy table
            DataTable table = new DataTable();
            table.Columns.Add(colValue);

            var column = new fmt.Column();
            column.Header.Load(colValue);
            column.Values.Add("abcdefghijklmnopqrstuvwxyz");
            column.Values.Add("azerty");

            var formatter = new fmt.ColumnFormatter(new fmt.HeaderFormatter(), new fmt.CellFormatter());

            var texts = formatter.Tabulize(column);
            //Check that all lines have the same length = 26
            foreach (var item in texts)
                Assert.That(item.Length, Is.EqualTo(26));
        }

        [Test]
        public void Tabulize_Empty_HeaderDisplayed()
        {
            // Design Dummy Column
            DataColumn colValue = new DataColumn("NumValue");
            colValue.ExtendedProperties.Add("NBi::Role", ColumnRole.Value);
            colValue.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            colValue.ExtendedProperties.Add("NBi::Tolerance", new NumericAbsoluteTolerance(new decimal(0.001)));

            // Design dummy table
            DataTable table = new DataTable();
            table.Columns.Add(colValue);

            var column = new fmt.Column();
            column.Header.Load(colValue);
            column.Values.Add(12345.67890);
            column.Values.Add(103.55);

            var formatter = new fmt.ColumnFormatter(new fmt.HeaderFormatter(), new fmt.CellFormatter());

            var texts = formatter.Tabulize(column);
            //Check that header is there
            var text = texts.Aggregate((i, j) => i + "|\r\n" + j);
            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.StringContaining("(+/- 0.001)"));
        }

        [Test]
        public void Tabulize_Empty_ContentDisplayed()
        {
            // Design Dummy Column
            DataColumn colValue = new DataColumn("NumValue");
            colValue.ExtendedProperties.Add("NBi::Role", ColumnRole.Value);
            colValue.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            colValue.ExtendedProperties.Add("NBi::Tolerance", new NumericAbsoluteTolerance(new decimal(0.001)));

            var column = new fmt.Column();
            column.Header.Load(colValue);
            column.Values.Add(12345.67890);
            column.Values.Add(103.55);

            var formatter = new fmt.ColumnFormatter(new fmt.HeaderFormatter(), new fmt.CellFormatter());

            var texts = formatter.Tabulize(column);
            var text = texts.Aggregate((i, j) => i + "|\r\n" + j);           

            //Check that content is there
            Assert.That(text, Is.StringContaining("12345.6789"));
            Assert.That(text, Is.StringContaining("103.55"));
        }
    }
}
