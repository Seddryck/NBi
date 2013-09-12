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

    }
}
