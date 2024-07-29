using NBi.Core.Scalar.Conversion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Conversion
{
    public class TextToDateTimeConverterTest
    {
        [Test]
        [TestCase("fr-fr")]
        [TestCase("en-us")]
        [TestCase("jp-jp")]
        [TestCase("ru-ru")]
        [TestCase("ko-ko")]
        public void Execute_ValidDateTime_Date(string culture)
        {
            var cultureInfo = new CultureInfo(culture, false);
            var text = (new DateTime(2018, 1, 6, 5, 12, 25)).ToString(cultureInfo.DateTimeFormat.ShortDatePattern + " " + cultureInfo.DateTimeFormat.LongTimePattern, cultureInfo.DateTimeFormat);

            var converter = new TextToDateTimeConverter(cultureInfo, DateTime.MinValue);
            var newValue = converter.Execute(text);
            Assert.That(newValue, Is.TypeOf<DateTime>());
            Assert.That(newValue, Is.EqualTo(new DateTime(2018, 01, 6, 5, 12, 25)));
        }

        [Test]
        [TestCase("06 Janvier 2018", "fr-fr")]
        [TestCase("06/01/2018", "fr-fr")]
        [TestCase("06-JAN", "en-us")]
        public void Execute_InvalidDate_Date(string text, string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            var converter = new TextToDateTimeConverter(cultureInfo, DateTime.MinValue);
            var newValue = converter.Execute(text);
            Assert.That(newValue, Is.TypeOf<DateTime>());
            Assert.That(newValue, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        [TestCase("06 Janvier 2018", "fr-fr")]
        [TestCase("06/01/2018", "fr-fr")]
        [TestCase("06-JAN", "en-us")]
        public void Execute_InvalidDate_Null(string text, string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            var converter = new TextToDateTimeConverter(cultureInfo, null);
            var newValue = converter.Execute(text);
            Assert.That(newValue, Is.Null);
        }
    }
}
