using System;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using NBi.Core.Scalar.Caster;
using NBi.Core;

namespace NBi.Testing.Unit.Core.Scalar.Caster
{
    [TestFixture]
    public class NumericCasterTest
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
        public void Execute_Decimal_Decimal()
        {
            Assert.That(new NumericCaster().Execute((new decimal(10))), Is.EqualTo(10));
        }

        [Test]
        [TestCase("10", 10)]
        [TestCase("10.25", 10.25)]
        [TestCase("10,000.1", 10000.1)]
        public void Execute_String_Decimal(string value, decimal expected)
        {
            Assert.That(new NumericCaster().Execute(value), Is.EqualTo(expected));
        }


        [Test]
        public void Execute_NonNumeric_ThrowNBiException()
        {
            Assert.Throws<NBiException>(() => new NumericCaster().Execute("zero"));
        }

    }
}
