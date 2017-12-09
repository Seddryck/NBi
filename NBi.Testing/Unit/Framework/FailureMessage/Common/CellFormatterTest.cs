using NBi.Core.ResultSet;
using NBi.Unit.Framework.FailureMessage.Common;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Framework.FailureMessage.Common
{
    public class CellFormatterTest
    {
        [Test]
        [TestCase(ColumnType.Text)]
        [TestCase(ColumnType.Numeric)]
        [TestCase(ColumnType.DateTime)]
        [TestCase(ColumnType.Boolean)]
        public void Format_NullValue_NullDisplay(ColumnType columnType)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(columnType);
            var text = formatter.Format(null);
            Assert.That(text, Is.EqualTo("(null)"));
        }

        [Test]
        [TestCase(ColumnType.Text)]
        [TestCase(ColumnType.Numeric)]
        [TestCase(ColumnType.DateTime)]
        [TestCase(ColumnType.Boolean)]
        public void Format_DBNullValue_NullDisplay(ColumnType columnType)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(columnType);
            var text = formatter.Format(DBNull.Value);
            Assert.That(text, Is.EqualTo("(null)"));
        }

        [Test]
        [TestCase(ColumnType.Text)]
        [TestCase(ColumnType.Numeric)]
        [TestCase(ColumnType.DateTime)]
        [TestCase(ColumnType.Boolean)]
        public void Format_StringNullValue_NullDisplay(ColumnType columnType)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(columnType);
            var text = formatter.Format("(null)");
            Assert.That(text, Is.EqualTo("(null)"));
        }

        [Test]
        [TestCase("", "(empty)")]
        [TestCase(" ", "(1 space)")]
        [TestCase("  ", "(2 spaces)")]
        [TestCase("MyText", "MyText")]
        [TestCase(10, "10")]
        [TestCase(true, "True")]
        public void Format_TextColumnObjectValue_CorrectDisplay(object value, string expected)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(ColumnType.Text);
            var text = formatter.Format(value);
            Assert.That(text, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(10.50, "10,5", "fr-fr")]
        [TestCase(10.50, "10.5", "en-us")]
        public void Format_TextColumnObjectValueCultureSpecific_CorrectDisplay(object value, string expected, string culture)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(ColumnType.Text);
            var text = formatter.Format(value);
            Assert.That(text, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(10, "10")]
        [TestCase(10.0f, "10")]
        [TestCase(10.40d, "10.4")]
        [TestCase("10.500", "10.500")]
        public void Format_NumericColumnObjectValue_CorrectDisplay(object value, string expected)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(ColumnType.Numeric);
            var text = formatter.Format(value);
            Assert.That(text, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("2010-07-04", "2010-07-04")]
        [TestCase("2010-07-04 00:00:00", "2010-07-04")]
        [TestCase("2010-07-04 11:30:10", "2010-07-04 11:30:10")]
        [TestCase("2010-07-04 11:30:10.000", "2010-07-04 11:30:10")]
        [TestCase("2010-07-04 11:30:10.325", "2010-07-04 11:30:10.325")]
        public void Format_DateTimeColumnObjectValue_CorrectDisplay(object value, string expected)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(ColumnType.DateTime);
            var text = formatter.Format(value);
            Assert.That(text, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource("DateTimeValues")]
        public void Format_DateTimeColumnDateTimeValue_CorrectDisplay(object value, string expected)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(ColumnType.DateTime);
            var text = formatter.Format(value);
            Assert.That(text, Is.EqualTo(expected));
        }


        private static IEnumerable DateTimeValues()
        {
            yield return new TestCaseData(new DateTime(2010, 7, 4), "2010-07-04");
            yield return new TestCaseData(new DateTime(2010, 7, 4, 11, 30, 10), "2010-07-04 11:30:10");
            yield return new TestCaseData(new DateTime(2010, 7, 4, 11, 30, 10, 325), "2010-07-04 11:30:10.325");
        }

        [Test]
        [TestCase(true)]
        [TestCase("TRUE")]
        [TestCase("true")]
        [TestCase(1)]
        public void Format_BooleanColumnObjectValueForTrue_DisplayIsTrue(object value)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(ColumnType.Boolean);
            var text = formatter.Format(value);
            Assert.That(text, Is.EqualTo("True"));
        }

        [Test]
        [TestCase(false)]
        [TestCase("FALSE")]
        [TestCase("false")]
        [TestCase(0)]
        public void Format_BooleanColumnObjectValueForFalse_DisplayIsFalse(object value)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(ColumnType.Boolean);
            var text = formatter.Format(value);
            Assert.That(text, Is.EqualTo("False"));
        }
    }
}
