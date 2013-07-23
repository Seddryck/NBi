using System;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Comparer
{
    [TestFixture]
    public class NumericComparerTest
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
        public void Compare_1DecimalAnd1Double_True()
        {
            var comparer = new NumericComparer();
            var result = comparer.Compare(new decimal(1), 1.0);
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_1DecimalAnd2Double_False()
        {
            var comparer = new NumericComparer();
            var result = comparer.Compare(new decimal(1), 2.0);
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_1DecimalAnd1String_True()
        {
            var comparer = new NumericComparer();
            var result = comparer.Compare(new decimal(1), "1.0");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_1DecimalAnd2String_False()
        {
            var comparer = new BooleanComparer();
            var result = comparer.Compare(new decimal(1), "2.0");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_1DecimalAnd1dot5Double_True()
        {
            var comparer = new NumericComparer();
            var result = comparer.Compare(new decimal(1), 1.5, 1);
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_1DecimalAnd2dot5Double_False()
        {
            var comparer = new NumericComparer();
            var result = comparer.Compare(new decimal(1), 2.5, 1);
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_1DecimalAnd1dot5String_True()
        {
            var comparer = new NumericComparer();
            var result = comparer.Compare(new decimal(1), "1.5", 1);
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_1DecimalAnd2dot5String_False()
        {
            var comparer = new NumericComparer();
            var result = comparer.Compare(new decimal(1), "2.5", 1);
            Assert.That(result.AreEqual, Is.False);
        }
    }
}
