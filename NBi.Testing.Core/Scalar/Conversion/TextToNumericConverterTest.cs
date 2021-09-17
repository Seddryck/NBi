using NBi.Core.Scalar.Conversion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Scalar.Conversion
{
    public class TextToNumericConverterTest
    {
        [Test]
        [TestCase("fr-fr")]
        [TestCase("en-us")]
        [TestCase("jp-jp")]
        [TestCase("ru-ru")]
        [TestCase("ko-ko")]
        public void Execute_ValidNumeric_Decimal(string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            var text = (100.456).ToString(cultureInfo.NumberFormat);

            var converter = new TextToNumericConverter(cultureInfo, -1m);
            var newValue = converter.Execute(text);
            Assert.That(newValue, Is.TypeOf<Decimal>());
            Assert.That(newValue, Is.EqualTo(new Decimal(100.456)));
        }

        [Test]
        [TestCase("100.456", "fr-fr")]
        [TestCase("100,456", "en-us")]
        public void Execute_InvalidNumeric_Decimal(string text, string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            var converter = new TextToNumericConverter(cultureInfo, -1m);
            var newValue = converter.Execute(text);
            Assert.That(newValue, Is.TypeOf<Decimal>());
            Assert.That(newValue, Is.EqualTo(-1));
        }

        [Test]
        [TestCase("100.456", "fr-fr")]
        [TestCase("100,456", "en-us")]
        public void Execute_InvalidNumeric_Null(string text, string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            var converter = new TextToNumericConverter(cultureInfo, null);
            var newValue = converter.Execute(text);
            Assert.That(newValue, Is.Null);
        }
    }
}
