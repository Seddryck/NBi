using System;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Comparer
{
    [TestFixture]
    public class DateTimeComparerTest
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
        public void Compare_xxxxyyyy_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("2013-10-16", new DateTime(2013, 10, 16));
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_ddmmyyyy_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("16/10/2013", new DateTime(2013, 10, 16));
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_mmddyyyy_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("10/16/2013", new DateTime(2013,10,16));
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_mmddyyyyhhmmss_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("10/16/2013 00:00:00", new DateTime(2013, 10, 16));
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_ValidDateAndAny_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("2013-10-16", "(any)");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_StringAndAny_ArgumentException()
        {
            var comparer = new DateTimeComparer();
            Assert.Throws<ArgumentException>(delegate {comparer.Compare("Not a date", "(any)");});
        }

        [Test]
        public void Compare_StringAndValue_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("2013-10-16", "(value)");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_NullAndAny_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare(null, "(any)");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_NullAndValue_False()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare(null, "(value)");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_NullAndString_False()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare(null, new DateTime(2013, 10, 16));
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_NullAndNullPlaceHolder_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare(null, "(null)");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_yyyymmddWithToleranceInDays_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("2013-10-09", "2013-10-08", "1");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_yyyymmddWithToleranceInDays_False()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("2013-10-09", "2013-10-01", "1");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_yyyymmddWithToleranceInHours_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("2013-10-09", "2013-10-08 06:00:00", "22:30:00");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_yyyymmddWithToleranceInHours_False()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("2013-10-09", "2013-10-08 01:00:00", "22:30:00");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_yyyymmddWithToleranceInMilliSeconds_True()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("2013-10-08 01:00:00.500", "2013-10-08 01:00:00.550", "00:00:00.125");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_yyyymmddWithToleranceInMilliSeconds_False()
        {
            var comparer = new DateTimeComparer();
            var result = comparer.Compare("2013-10-08 01:00:00.500", "2013-10-08 01:00:00.850", "00:00:00.125");
            Assert.That(result.AreEqual, Is.False);
        }
    }
}
