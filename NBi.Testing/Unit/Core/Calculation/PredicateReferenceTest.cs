using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using NBi.Core.ResultSet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Calculation
{
    public class PredicateReferenceTest
    {
        [Test]
        [TestCase(ComparerType.Equal, "A", "A")]
        [TestCase(ComparerType.Equal, "", "(empty)")]
        [TestCase(ComparerType.Equal, "A", "(value)")]
        [TestCase(ComparerType.LessThan, "A", "B")]
        [TestCase(ComparerType.LessThanOrEqual, "A", "B")]
        [TestCase(ComparerType.LessThanOrEqual, "A", "A")]
        [TestCase(ComparerType.MoreThan, "V", "B")]
        [TestCase(ComparerType.MoreThanOrEqual, "V", "B")]
        [TestCase(ComparerType.MoreThanOrEqual, "V", "V")]
        [TestCase(ComparerType.StartsWith, "Paris", "P")]
        [TestCase(ComparerType.EndsWith, "Paris", "s")]
        [TestCase(ComparerType.Contains, "Paris", "ar")]
        public void Compare_Text_Success(ComparerType comparerType, object x, object y)
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType== ColumnType.Text
                    && i.ComparerType == comparerType
                    && i.Reference == y
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Apply(x), Is.True);
        }

        [Test]
        [TestCase(ComparerType.Equal, "A", "B")]
        [TestCase(ComparerType.LessThan, "A", "(empty)")]
        [TestCase(ComparerType.LessThanOrEqual, "C", "B")]
        [TestCase(ComparerType.MoreThan, "A", "B")]
        [TestCase(ComparerType.MoreThanOrEqual, "A", "B")]
        [TestCase(ComparerType.StartsWith, "Paris", "p")]
        [TestCase(ComparerType.EndsWith, "Paris", "i")]
        [TestCase(ComparerType.Contains, "Paris", "mar")]
        public void Compare_Text_Failure(ComparerType comparerType, object x, object y)
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType == ColumnType.Text
                    && i.ComparerType == comparerType
                    && i.Reference == y
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Apply(x), Is.False);
        }

        [Test]
        public void Compare_TextNull_Success()
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType == ColumnType.Text
                    && i.ComparerType == ComparerType.Equal
                    && i.Reference == (object)"(null)"
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Apply(null), Is.True);
        }

        [Test]
        [TestCase(ComparerType.Equal, 1, 1)]
        [TestCase(ComparerType.Equal, 1, 1.0)]
        [TestCase(ComparerType.Equal, 1, "(value)")]
        [TestCase(ComparerType.LessThan, 1, 10)]
        [TestCase(ComparerType.LessThanOrEqual, 1, 10)]
        [TestCase(ComparerType.LessThanOrEqual, 1, "10.0")]
        [TestCase(ComparerType.LessThanOrEqual, 1, 1)]
        [TestCase(ComparerType.MoreThan, 10, 1)]
        [TestCase(ComparerType.MoreThanOrEqual, 10, 1)]
        [TestCase(ComparerType.MoreThanOrEqual, 1, 1)]
        [TestCase(ComparerType.MoreThanOrEqual, 1, "1.00")]
        public void Compare_Numeric_Success(ComparerType comparerType, object x, object y)
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType == ColumnType.Numeric
                    && i.ComparerType == comparerType
                    && i.Reference == y
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Apply(x), Is.True);
        }

        [Test]
        [TestCase(ComparerType.Equal, 10, 10)]
        [TestCase(ComparerType.LessThan, 10, 12)]
        [TestCase(ComparerType.LessThanOrEqual, 10, 12)]
        [TestCase(ComparerType.LessThanOrEqual, 10, 10)]
        [TestCase(ComparerType.MoreThan, 12, 10)]
        [TestCase(ComparerType.MoreThanOrEqual, 12, 10)]
        [TestCase(ComparerType.MoreThanOrEqual, 10, 10)]
        public void Compare_DateTime_Success(ComparerType comparerType, int x, int y)
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType == ColumnType.DateTime
                    && i.ComparerType == comparerType
                    && i.Reference == (object)new DateTime(2015, y, 1)
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Apply(new DateTime(2015, x, 1)), Is.True);
        }

        [Test]
        [TestCase(ComparerType.Equal, true, true)]
        [TestCase(ComparerType.Equal, "true", true)]
        [TestCase(ComparerType.Equal, "Yes", true)]
        public void Compare_Boolean_Success(ComparerType comparerType, object x, object y)
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType == ColumnType.Boolean
                    && i.ComparerType == comparerType
                    && i.Reference == y
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Apply(x), Is.True);
        }

        [Test]
        [TestCase(ComparerType.LessThan)]
        [TestCase(ComparerType.LessThanOrEqual)]
        [TestCase(ComparerType.MoreThan)]
        [TestCase(ComparerType.MoreThanOrEqual)]
        public void Compare_Boolean_ThrowsArgumentException(ComparerType comparerType)
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType == ColumnType.Boolean
                    && i.ComparerType == comparerType
                );

            var factory = new PredicateFactory();
            Assert.Throws<ArgumentOutOfRangeException>(delegate { factory.Get(info); });
        }
    }
}
