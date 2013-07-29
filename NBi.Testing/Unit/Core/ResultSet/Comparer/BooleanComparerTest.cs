using System;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Comparer
{
    [TestFixture]
    public class BooleanComparerTest
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
        public void Compare_1DecimalAndStringTrue_True()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(new decimal(1), "True");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_1IntegerAndStringTrue_True()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(1, "True");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_ZeroIntegerAndFalse_True()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(0, false);
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_1dot0AndStringTrue_True()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(1.0, "True");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_1dot0AndStringFalse_False()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(1.0, "False");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_TrueAndStringFalse_False()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(true, "False");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_TrueAndTrue_True()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(true, true);
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_TrueAndAnyString_False()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(true, "string");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_StringTrueLowerCaseAndStringTrueUpperCase_True()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare("true", "TRUE");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_2AndStringTrue_False()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(new decimal(2), "True");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_2AndStringFalse_False()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(new decimal(2), "False");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_StringAndOtherString_False()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare("string", "other string");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_YesAndTrue_True()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare("Yes", true);
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_YesAndFalse_False()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare("Yes", false);
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_NoAndTrue_False()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare("No", true);
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_NoAndFalse_True()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare("No", false);
            Assert.That(result.AreEqual, Is.True);
        }
    }
}
