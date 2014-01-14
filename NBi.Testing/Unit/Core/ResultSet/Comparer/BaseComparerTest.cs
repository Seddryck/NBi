using System;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Comparer
{
    [TestFixture]
    public class BaseComparerTest
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
        public void IsValidNumeric_Decimal_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric((new decimal(10))), Is.True);
        }

        [Test]
        public void IsValidNumeric_Int_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric(10), Is.True);
        }

        [Test]
        public void IsValidNumeric_Double_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric(10.5), Is.True);
        }

        [Test]
        public void IsValidNumeric_NegativeDouble_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric(-10.5), Is.True);
        }

        [Test]
        public void IsValidNumeric_StringInt_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric("10"), Is.True);
        }

        [Test]
        public void IsValidNumeric_StringDouble_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric("10.5"), Is.True);
        }

        [Test]
        public void IsValidNumeric_NegativeStringDouble_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric("-10.5"), Is.True);
        }

        [Test]
        public void IsValidNumeric_ObjectDouble_True()
        {
            var comparer = new BaseComparer();
            object obj = "10.5";
            Assert.That(comparer.IsValidNumeric(obj), Is.True);
        }

        [Test]
        [Ignore]
        public void IsValidNumeric_StringDoubleAlternativeDecimalSeparator_False()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric("10,5"), Is.False);
        }

        [Test]
        public void IsValidNumeric_StringDoubleMultipleDecimalSeparator_False()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric("10.000,5"), Is.False);
        }

        [Test]
        public void IsValidNumeric_String_False()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric("Numeric"), Is.False);
        }

        [Test]
        public void IsValidNumeric_Date_False()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidNumeric("10/10/2013"), Is.False);
        }

        //Valid dates
        [Test]
        public void IsValidDateTime_xxxxyyyy_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("10/10/2013"), Is.True);
        }

        [Test]
        public void IsValidDateTime_ddmmyyyy_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("16/10/2013"), Is.True);
        }

        [Test]
        public void IsValidDateTime_mmddyyyy_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("10/16/2013"), Is.True);
        }

        [Test]
        public void IsValidDateTime_ddmmyy_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("16/10/13"), Is.True);
        }

        [Test]
        public void IsValidDateTime_mmddyy_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("10/16/13"), Is.True);
        }

        [Test]
        public void IsValidDateTime_dmyy_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("5.12.78"), Is.True);
        }

        [Test]
        public void IsValidDateTime_ddmyy_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("10.5.2013"), Is.True);
        }

        [Test]
        public void IsValidDateTime_yyyymmdd_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("2013-10-16"), Is.True);
        }

        [Test]
        public void IsValidDateTime_ddmm_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("16.12"), Is.True);
        }

        [Test]
        public void IsValidDateTime_mmdd_True()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("12/17"), Is.True);
        }

        [Test]
        public void IsValidDateTime_String_False()
        {
            var comparer = new BaseComparer();
            Assert.That(comparer.IsValidDateTime("DateTime"), Is.False);
        }
    }
}
