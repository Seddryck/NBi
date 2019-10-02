using NBi.Core.Transformation.Transformer.Native;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Transformation.Transformer.Native
{
    [TestFixture]
    public class TextTest
    {
        [Test]
        [TestCase("123456789", 9, "123456789")]
        [TestCase("123456789", 10, "123456789")]
        [TestCase("123456789", 8, "12345678")]
        [TestCase("123456789", 0, "")]
        [TestCase("(null)", 3, "(null)")]
        [TestCase("(empty)", 3, "(empty)")]
        public void Execute_TextToFirstChars_Valid(string value, int length, string expected)
        {
            var function = new TextToFirstChars(length.ToString());
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("123456789", 9, "123456789")]
        [TestCase("123456789", 10, "123456789")]
        [TestCase("123456789", 8, "23456789")]
        [TestCase("123456789", 0, "")]
        [TestCase("(null)", 3, "(null)")]
        [TestCase("(empty)", 3, "(empty)")]
        public void Execute_TextToLastChars_Valid(string value, int length, string expected)
        {
            var function = new TextToLastChars(length.ToString());
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1234", 9, "0", "123400000")]
        [TestCase("1234", 9, "*", "1234*****")]
        [TestCase("123456789", 3, "0", "123456789")]
        [TestCase("(null)", 3, "0", "000")]
        [TestCase("(empty)", 3, "0", "000")]
        public void Execute_TextToPadRight_Valid(string value, int length, string character, string expected)
        {
            var function = new TextToPadRight(length.ToString(), character);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("1234", 9, "0", "000001234")]
        [TestCase("1234", 9, "*", "*****1234")]
        [TestCase("123456789", 3, "0", "123456789")]
        [TestCase("(null)", 3, "0", "000")]
        [TestCase("(empty)", 3, "0", "000")]
        public void Execute_TextToPadLeft_Valid(string value, int length, string character, string expected)
        {
            var function = new TextToPadLeft(length.ToString(), character);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        [TestCase("20190317111223", "yyyyMMddhhmmss", "2019-03-17 11:12:23")]
        [TestCase("2019-03-17 11:12:23", "yyyy-MM-dd hh:mm:ss", "2019-03-17 11:12:23")]
        [TestCase("17-03-2019 11:12:23", "dd-MM-yyyy hh:mm:ss", "2019-03-17 11:12:23")]
        [TestCase("2019-03-17T11:12:23", "yyyy-MM-ddThh:mm:ss", "2019-03-17 11:12:23")]
        [TestCase("17/03/2019 11:12:23", "dd/MM/yyyy hh:mm:ss", "2019-03-17 11:12:23")]
        [TestCase("17.03.2019 11.12.23", "dd.MM.yyyy hh.mm.ss", "2019-03-17 11:12:23")]
        [TestCase("Wed, 25.09.19", "ddd, dd.MM.yy", "2019-09-25")]
        [TestCase("Wednesday 25-SEP-19", "dddd dd-MMM-yy", "2019-09-25")]
        [TestCase("2019-10-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "2019-10-01 19:58:00")]
        public void Execute_TextToDateTime_Valid(string value, string format, DateTime expected)
        {
            var function = new TextToDateTime(format);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
            Assert.That(((DateTime) result).Kind, Is.EqualTo(DateTimeKind.Unspecified));
        }

        [Test]
        [TestCase("2019-11-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "Brussels", "2019-11-01 20:58:00")]
        [TestCase("2019-10-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "Brussels", "2019-10-01 21:58:00")]
        [TestCase("2019-10-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "Moscow", "2019-10-01 22:58:00")]
        [TestCase("2019-10-01T19:58Z", "yyyy-MM-ddTHH:mmZ", "Pacific Standard Time", "2019-10-01 12:58:00")]
        public void Execute_TextToDateTimeAndUtcToLocal_Valid(string value, string format, string timeZone, DateTime expected)
        {
            var textToDateTime = new TextToDateTime(format);
            var utcToLocal = new UtcToLocal(timeZone);
            var result = utcToLocal.Evaluate(textToDateTime.Evaluate(value));
            Assert.That(result, Is.EqualTo(expected));
            Assert.That(((DateTime)result).Kind, Is.EqualTo(DateTimeKind.Unspecified));
        }

        [Test]
        [TestCase("20190317111223", "yyyyMMddhhmmss", "fr-fr", "2019-03-17 11:12:23")]
        [TestCase("mercredi 25-sept.-19", "dddd dd-MMM-yy", "fr-fr", "2019-09-25")]
        public void Execute_TextToDateTimeWithCulture_Valid(string value, string format, string culture, DateTime expected)
        {
            var function = new TextToDateTime(format, culture);
            var result = function.Evaluate(value);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
