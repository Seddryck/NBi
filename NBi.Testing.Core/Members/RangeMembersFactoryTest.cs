using System;
using System.Globalization;
using System.Linq;
using NBi.Core.Members.Ranges;
using NUnit.Framework;

namespace NBi.Core.Testing.Members
{
    [TestFixture]
    public class RangeMembersFactoryTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        private class IntegerRange : IIntegerRange
        {
            public int Start { get; set; }
            public int End { get; set; }
            public int Step { get; set; }
        }

        private class DateRange : IDateRange
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
            public string Format { get; set; } = string.Empty;
        }

        private class IntegerPatternRange : IIntegerRange, IPatternDecorator
        {
            public int Start { get; set; }
            public int End { get; set; }
            public int Step { get; set; }

            public string Pattern { get; set; } = string.Empty;
            public PositionValue Position { get; set; }
        }

        [Test]
        public void Instantiate_IntegerFrom3To10_ListFrom3To10()
        {
            var integerRange = new IntegerRange()
                {
                    Start = 3,
                    End = 10,
                    Step = 1
                };

            var factory = new RangeMembersFactory();
            var values = factory.Instantiate(integerRange).ToList();

            Assert.That(values[0], Is.EqualTo("3"));
            Assert.That(values[7], Is.EqualTo("10"));
            Assert.That(values.Count, Is.EqualTo(8));
        }

        [Test]
        public void Instantiate_IntegerFrom3To10Step3_ListFrom3To9()
        {
            var integerRange = new IntegerRange()
            {
                Start = 3,
                End = 10,
                Step = 3
            };

            var factory = new RangeMembersFactory();
            var values = factory.Instantiate(integerRange).ToList();

            Assert.That(values[0], Is.EqualTo("3"));
            Assert.That(values[2], Is.EqualTo("9"));
            Assert.That(values.Count, Is.EqualTo(3));
        }

        [Test]
        public void Instantiate_IntegerFrom1To52WithPrefixPattern_ListFromWeek1ToWeek52()
        {
            var integerRange = new IntegerPatternRange()
            {
                Start = 1,
                End = 52,
                Step = 1,
                Pattern="Week ",
                Position= PositionValue.Prefix
            };

            var factory = new RangeMembersFactory();
            var values = factory.Instantiate(integerRange).ToList();

            Assert.That(values[0], Is.EqualTo("Week 1"));
            Assert.That(values[51], Is.EqualTo("Week 52"));
            Assert.That(values.Count, Is.EqualTo(52));
        }

        [Test]
        [TestCase("en")]
        [TestCase("en-us")]
        [TestCase("fr")]
        [TestCase("fr-be")]
        [TestCase("fr-fr")]
        [TestCase("nl")]
        [TestCase("nl-be")]
        [TestCase("de")]
        public void Instantiate_DateFrom1stJanuaryTo31December2013_ListWithAllDays(string cultureTag)
        {
            var culture = new CultureInfo(cultureTag, false);
            var dateRange = new DateRange()
            {
                Start = new DateTime(2013, 1, 1),
                End = new DateTime(2013, 12, 31),
                Culture = culture
            };

            var factory = new RangeMembersFactory();
            var values = factory.Instantiate(dateRange).ToList();

            Assert.That(values.Count, Is.EqualTo(365));

            var januaryFirst = new DateTime(2013, 1, 1).ToString(culture.DateTimeFormat.ShortDatePattern);
            Assert.That(values[0], Is.EqualTo(januaryFirst));
            var decemberLast = new DateTime(2013, 12, 31).ToString(culture.DateTimeFormat.ShortDatePattern);
            Assert.That(values[364], Is.EqualTo(decemberLast));

        }

        [Test]
        public void Instantiate_DateFrom1stJanuaryTo31December2012InFrenchWithLongFormat_ListWithAllDays()
        {
            var dateRange = new DateRange()
            {
                Start = new DateTime(2012, 1, 1),
                End = new DateTime(2012, 12, 31),
                Culture = new CultureInfo("fr"),
                Format = "MMMM dd, yyyy"
            };

            var factory = new RangeMembersFactory();
            var values = factory.Instantiate(dateRange).ToList();

            Assert.That(values.Count, Is.EqualTo(366));
            Assert.That(values[0], Is.EqualTo("janvier 01, 2012"));
            Assert.That(values[365], Is.EqualTo("décembre 31, 2012"));

        }

    }
}
