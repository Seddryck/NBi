using NBi.Core.Scalar.Duration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Duration
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

            Assert.That(converted, Is.Not.Null);
            Assert.That(converted, Is.AssignableTo<IDuration>());
            Assert.That(converted, Is.TypeOf<MonthDuration>());
            Assert.That(((MonthDuration)converted!).Count, Is.EqualTo(expected));
        }

        [TestCase(1, "1 month")]
        [TestCase(2, "2 months")]
        [TestCase(10, "10 months")]
        public void ConvertTo_MonthDuration_StringMonth(int value, string expected)
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
        public void ConvertFrom_ValidStringYear_YearDuration(string value, int expected)
        {
            var converter = new DurationConverter();
            var converted = converter.ConvertFrom(value);

            Assert.That(converted, Is.Not.Null);
            Assert.That(converted, Is.AssignableTo<IDuration>());
            Assert.That(converted, Is.TypeOf<YearDuration>());
            Assert.That(((YearDuration)converted!).Count, Is.EqualTo(expected));
        }

        [TestCase(1, "1 year")]
        [TestCase(2, "2 years")]
        [TestCase(10, "10 years")]
        public void ConvertTo_YearDuration_StringYear(int value, string expected)
        {
            var converter = new DurationConverter();
            var converted = converter.ConvertTo(new YearDuration(value), typeof(string));

            Assert.That(converted, Is.TypeOf<string>());
            Assert.That(converted, Is.EqualTo(expected));
        }

        [TestCase("1 day", 24)]
        [TestCase("3 days", 72)]
        [TestCase("1.00:00:00", 24)]
        [TestCase("1.01:00:00", 25)]
        [TestCase("02:00:00", 2)]
        [TestCase("17:00:00", 17)]
        public void ConvertFrom_ValidStringDuration_TotalHours(string value, int expected)
        {
            var converter = new DurationConverter();
            var converted = converter.ConvertFrom(value);

            Assert.That(converted, Is.Not.Null);
            Assert.That(converted, Is.AssignableTo<IDuration>());
            Assert.That(converted, Is.TypeOf<FixedDuration>());
            Assert.That(((FixedDuration)converted!).TimeSpan.TotalHours, Is.EqualTo(expected));
        }

        [TestCase(1, "01:00:00")]
        [TestCase(2, "02:00:00")]
        [TestCase(24, "1.00:00:00")]
        [TestCase(56, "2.08:00:00")]
        public void ConvertTo_TotalHours_FixedDuration(int value, string expected)
        {
            var converter = new DurationConverter();
            var converted = converter.ConvertTo(new FixedDuration(new TimeSpan(0, value, 0, 0)), typeof(string));

            Assert.That(converted, Is.TypeOf<string>());
            Assert.That(converted, Is.EqualTo(expected));
        }

    }
}
