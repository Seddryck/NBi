using System;
using System.Globalization;
using System.Linq;
using System.Linq;
using NBi.Core.Members.Ranges;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Members
{
    [TestFixture]
    public class RangeMembersFactoryTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
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
        public void Instantiate_DateFrom1stJanuaryTo31December2013_ListWithAllDays()
        {
            var dateRange = new DateRange()
            {
                Start = new DateTime(2013,1,1),
                End = new DateTime(2013, 12, 31),
                Culture = new CultureInfo("en")
            };

            var factory = new RangeMembersFactory();
            var values = factory.Instantiate(dateRange).ToList();

            Assert.That(values.Count, Is.EqualTo(365));
            Assert.That(values[0], Is.EqualTo("1/1/2013"));
            Assert.That(values[364], Is.EqualTo("12/31/2013"));
            
        }

        [Test]
        public void Instantiate_DateFrom1stJanuaryTo31December2013InFrench_ListWithAllDays()
        {
            var dateRange = new DateRange()
            {
                Start = new DateTime(2013, 1, 1),
                End = new DateTime(2013, 12, 31),
                Culture = new CultureInfo("fr-be")
            };

            var factory = new RangeMembersFactory();
            var values = factory.Instantiate(dateRange).ToList();

            Assert.That(values.Count, Is.EqualTo(365));
            Assert.That(values[0], Is.EqualTo("1/01/2013"));
            Assert.That(values[364], Is.EqualTo("31/12/2013"));

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
