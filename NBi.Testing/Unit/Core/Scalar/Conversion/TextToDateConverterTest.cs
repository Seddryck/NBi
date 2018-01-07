using NBi.Core.Scalar.Conversion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Scalar.Conversion
{
    public class TextToDateConverterTest
    {
        [Test]
        [TestCase("06/01/2018", "fr-fr")]
        [TestCase("06-JAN-18", "en-us")]
        public void Execute_ValidDate_Date(string text, string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            var converter = new TextToDateConverter(cultureInfo, DateTime.MinValue);
            var newValue = converter.Execute(text);
            Assert.That(newValue, Is.TypeOf<DateTime>());
            Assert.That(newValue, Is.EqualTo(new DateTime(2018, 01, 6)));
        }

        [Test]
        [TestCase("06 Janvier 2018", "fr-fr")]
        [TestCase("06-JAN", "en-us")]
        public void Execute_InvalidDate_Date(string text, string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            var converter = new TextToDateConverter(cultureInfo, DateTime.MinValue);
            var newValue = converter.Execute(text);
            Assert.That(newValue, Is.TypeOf<DateTime>());
            Assert.That(newValue, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        [TestCase("06 Janvier 2018", "fr-fr")]
        [TestCase("06-JAN", "en-us")]
        public void Execute_InvalidDate_Null(string text, string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            var converter = new TextToDateConverter(cultureInfo, null);
            var newValue = converter.Execute(text);
            Assert.That(newValue, Is.Null);
        }
    }
}
