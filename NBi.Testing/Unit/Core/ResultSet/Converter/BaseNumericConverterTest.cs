using System;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;
using NBi.Core.ResultSet.Converter;

namespace NBi.Testing.Unit.Core.ResultSet.Converter
{
    [TestFixture]
    public class BaseNumericConverterTest
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
        public void IsValid_Decimal_True()
        {
            
            Assert.That(new BaseNumericConverter().IsValid((new decimal(10))), Is.True);
        }

        [Test]
        public void IsValid_Int_True()
        {
            
            Assert.That(new BaseNumericConverter().IsValid(10), Is.True);
        }

        [Test]
        public void IsValid_Double_True()
        {
            
            Assert.That(new BaseNumericConverter().IsValid(10.5), Is.True);
        }

        [Test]
        public void IsValid_NegativeDouble_True()
        {
            
            Assert.That(new BaseNumericConverter().IsValid(-10.5), Is.True);
        }

        [Test]
        public void IsValid_StringInt_True()
        {
            
            Assert.That(new BaseNumericConverter().IsValid("10"), Is.True);
        }

        [Test]
        public void IsValid_StringDouble_True()
        {
            
            Assert.That(new BaseNumericConverter().IsValid("10.5"), Is.True);
        }

        [Test]
        public void IsValid_NegativeStringDouble_True()
        {
            
            Assert.That(new BaseNumericConverter().IsValid("-10.5"), Is.True);
        }

        [Test]
        public void IsValid_ObjectDouble_True()
        {
            
            object obj = "10.5";
            Assert.That(new BaseNumericConverter().IsValid(obj), Is.True);
        }

        [Test]
        [Ignore]
        public void IsValid_StringDoubleAlternativeDecimalSeparator_False()
        {
            
            Assert.That(new BaseNumericConverter().IsValid("10,5"), Is.False);
        }

        [Test]
        public void IsValid_StringDoubleMultipleDecimalSeparator_False()
        {
            
            Assert.That(new BaseNumericConverter().IsValid("10.000,5"), Is.False);
        }

        [Test]
        public void IsValid_String_False()
        {
            
            Assert.That(new BaseNumericConverter().IsValid("Numeric"), Is.False);
        }

        [Test]
        public void IsValid_Date_False()
        {
            
            Assert.That(new BaseNumericConverter().IsValid("10/10/2013"), Is.False);
        }
    }
}
