using System;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Comparer
{
    [TestFixture]
    public class TextComparerTest
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
        public void Compare_StringAndSameString_True()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare("string", "string");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_StringAndOtherString_False()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare("string", "other string");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_StringAndSameStringUppercase_False()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare("string", "STRING");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_StringAndSubstring_False()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare("string", "str");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_StringAndAny_True()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare("string", "(any)");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_StringAndValue_True()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare("string", "(value)");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_NullAndAny_True()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare(null, "(any)");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_NullAndValue_False()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare(null, "(value)");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_NullAndString_False()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare(null, "string");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_NullAndNullPlaceHolder_True()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare(null, "(null)");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_EmptyAndEmptyPlaceHolder_True()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare(string.Empty, "(empty)");
            Assert.That(result.AreEqual, Is.True);
        }

        [Test]
        public void Compare_NonEmptyAndEmptyPlaceHolder_False()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare("string", "(empty)");
            Assert.That(result.AreEqual, Is.False);
        }


        [Test]
        public void Compare_NullAndEmptyPlaceHolder_False()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare(null, "(empty)");
            Assert.That(result.AreEqual, Is.False);
        }

        [Test]
        public void Compare_1AndEmptyPlaceHolder_False()
        {
            var comparer = new TextComparer();
            var result = comparer.Compare(1, "(empty)");
            Assert.That(result.AreEqual, Is.False);
        }
    }
}
