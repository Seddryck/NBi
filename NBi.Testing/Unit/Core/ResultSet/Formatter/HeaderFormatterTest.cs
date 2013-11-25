using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Core.ResultSet.Formatter;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Formatter
{
    [TestFixture]
    public class HeaderFormatterTest
    {

        [Test]
        public void Tabulize_NumericAbsoluteTolerance_CorrectHeader()
        {
            // Design Dummy Column
            DataColumn col = new DataColumn("DummyColumn");
            col.ExtendedProperties.Add("NBi::Role", ColumnRole.Value);
            col.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            col.ExtendedProperties.Add("NBi::Tolerance", new NumericAbsoluteTolerance(new decimal(0.5)));

            var header = new Header();
            header.Load(col);
            var formatter = new HeaderFormatter();

            var texts = formatter.Tabulize(header, 50);
            var text = texts.Aggregate((i, j) => i + "|\r\n" + j);
            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.StringContaining("(+/- 0.5)"));
        }

        [Test]
        public void Tabulize_NumericPercentageTolerance_CorrectHeader()
        {
            // Design Dummy Column
            DataColumn col = new DataColumn("DummyColumn");
            col.ExtendedProperties.Add("NBi::Role", ColumnRole.Value);
            col.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            col.ExtendedProperties.Add("NBi::Tolerance", new NumericPercentageTolerance(new decimal(12.5)));

            var header = new Header();
            header.Load(col);
            var formatter = new HeaderFormatter();

            var texts = formatter.Tabulize(header, 50);
            var text = texts.Aggregate((i, j) => i + "|\r\n" + j);
            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.StringContaining("(+/- 12.5%)"));
        }

        [Test]
        public void Tabulize_DateTimeTolerance_CorrectHeader()
        {
            // Design Dummy Column
            DataColumn col = new DataColumn("DummyColumn");
            col.ExtendedProperties.Add("NBi::Role", ColumnRole.Value);
            col.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            col.ExtendedProperties.Add("NBi::Tolerance", new DateTimeTolerance(new TimeSpan(0, 15, 0)));
            var header = new Header();
            header.Load(col);
            var formatter = new HeaderFormatter();

            var texts = formatter.Tabulize(header, 50);
            var text = texts.Aggregate((i, j) => i + "|\r\n" + j);
            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.StringContaining("(+/- 00:15:00)"));
        }

        [Test]
        public void Tabulize_DateTimeRounding_CorrectHeader()
        {
            // Design Dummy Column
            DataColumn col = new DataColumn("DummyColumn");
            col.ExtendedProperties.Add("NBi::Role", ColumnRole.Value);
            col.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            col.ExtendedProperties.Add("NBi::Rounding", new DateTimeRounding(new TimeSpan(0, 15, 0), Rounding.RoundingStyle.Floor));

            var header = new Header();
            header.Load(col);
            var formatter = new HeaderFormatter();

            var texts = formatter.Tabulize(header, 50);
            var text = texts.Aggregate((i, j) => i + "|\r\n" + j);
            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.StringContaining("(floor 00:15:00)"));
        }

        [Test]
        public void Tabulize_NumericRounding_CorrectHeader()
        {
            // Design Dummy Column
            DataColumn col = new DataColumn("DummyColumn");
            col.ExtendedProperties.Add("NBi::Role", ColumnRole.Value);
            col.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            col.ExtendedProperties.Add("NBi::Rounding", new NumericRounding(10.5, Rounding.RoundingStyle.Round));

            var header = new Header();
            header.Load(col);
            var formatter = new HeaderFormatter();

            var texts = formatter.Tabulize(header, 50);
            var text = texts.Aggregate((i, j) => i + "|\r\n" + j);
            Assert.That(text, Is.StringContaining("VALUE"));
            Assert.That(text, Is.StringContaining("Numeric"));
            Assert.That(text, Is.StringContaining("(round 10.5)"));
        }

    }
}
