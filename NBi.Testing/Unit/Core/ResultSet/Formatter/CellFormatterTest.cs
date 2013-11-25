using System;
using System.Linq;
using NBi.Core.ResultSet.Formatter;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Formatter
{
    [TestFixture]
    public class CellFormatterTest
    {
        [Test]
        public void GetDisplay_Empty_CorrectDisplay()
        {
            var formatter = new CellFormatter();
            var text = formatter.GetDisplay(string.Empty);

            Assert.That(text, Is.EqualTo("(empty)"));
        }

        [Test]
        public void GetDisplay_Null_CorrectDisplay()
        {
            var formatter = new CellFormatter();
            var text = formatter.GetDisplay(DBNull.Value);

            Assert.That(text, Is.EqualTo("(null)"));
        }

        [Test]
        public void GetDisplay_Date_CorrectDisplay()
        {
            var formatter = new CellFormatter();
            var text = formatter.GetDisplay(new DateTime(2012,12,25));

            Assert.That(text, Is.EqualTo("2012-12-25 00:00:00"));
        }

        [Test]
        public void GetDisplay_DateTime_CorrectDisplay()
        {
            var formatter = new CellFormatter();
            var text = formatter.GetDisplay(new DateTime(2012, 12, 25, 17,42,55));

            Assert.That(text, Is.EqualTo("2012-12-25 17:42:55"));
        }


        [Test]
        public void GetDisplay_Decimal_CorrectDisplay()
        {
            var formatter = new CellFormatter();
            var text = formatter.GetDisplay(new Decimal(17.16454));

            Assert.That(text, Is.EqualTo("17.16454"));
        }


        [Test]
        public void GetDisplay_Double_CorrectDisplay()
        {
            var formatter = new CellFormatter();
            var text = formatter.GetDisplay(17.16001000);

            Assert.That(text, Is.EqualTo("17.16001"));
        }

        [Test]
        public void GetDisplay_Integer_CorrectDisplay()
        {
            var formatter = new CellFormatter();
            var text = formatter.GetDisplay(new Decimal(17));

            Assert.That(text, Is.EqualTo("17"));
        }


        [Test]
        public void GetDisplay_Boolean_CorrectDisplay()
        {
            var formatter = new CellFormatter();
            var text = formatter.GetDisplay(true);

            Assert.That(text, Is.EqualTo("True"));
        }
    }
}
