using NBi.Core.Scalar.Duration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Scalar.Duration
{
    public class DurationConverterTest
    {
        [TestCase("1 month", 1)]
        [TestCase("1 months", 1)]
        [TestCase("2 month", 2)]
        [TestCase("2 months", 2)]
        [TestCase("10 months", 10)]
        public void ConvertFrom_ValidStringMonth_MonthDuration(string value, int expected)
        {
            var converter = new DurationConverter();
            var converted = converter.ConvertFrom(value);

            Assert.That(converted, Is.AssignableTo<IDuration>());
            Assert.That(converted, Is.TypeOf<MonthDuration>());
            Assert.That((converted as MonthDuration).Count, Is.EqualTo(expected));
        }

        [TestCase(1, "1 month")]
        [TestCase(2, "2 months")]
        [TestCase(10, "10 months")]
        public void ConvertTo_ValidStringMonth_yearDuration(int value, string expected)
        {
            var converter = new DurationConverter();
            var converted = converter.ConvertTo(new MonthDuration(value), typeof(string));

            Assert.That(converted, Is.TypeOf<string>());
            Assert.That(converted, Is.EqualTo(expected));
        }

        [TestCase("1 year", 1)]
        [TestCase("1 years", 1)]
        [TestCase("2 year", 2)]
        [TestCase("2 years", 2)]
        [TestCase("10 years", 10)]
        public void ConvertFrom_ValidStringyear_yearDuration(string value, int expected)
        {
            var converter = new DurationConverter();
            var converted = converter.ConvertFrom(value);

            Assert.That(converted, Is.AssignableTo<IDuration>());
            Assert.That(converted, Is.TypeOf<YearDuration>());
            Assert.That((converted as YearDuration).Count, Is.EqualTo(expected));
        }

        [TestCase(1, "1 year")]
        [TestCase(2, "2 years")]
        [TestCase(10, "10 years")]
        public void ConvertTo_ValidStringYear_yearDuration(int value, string expected)
        {
            var converter = new DurationConverter();
            var converted = converter.ConvertTo(new YearDuration(value), typeof(string));

            Assert.That(converted, Is.TypeOf<string>());
            Assert.That(converted, Is.EqualTo(expected));
        }

    }
}
