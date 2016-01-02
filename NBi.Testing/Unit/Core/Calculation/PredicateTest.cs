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
    public class PredicateTest
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
        public void Compare_Text_Success(ComparerType comparerType, object x, object y)
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType== ColumnType.Text
                    && i.ComparerType == comparerType
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Compare(x, y), Is.True);
        }

        public void Compare_TextNull_Success()
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType == ColumnType.Text
                    && i.ComparerType == ComparerType.Equal
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Compare(null, "(null)"), Is.True);
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
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Compare(x, y), Is.True);
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
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Compare(new DateTime(2015, x, 1), new DateTime(2015, y, 1)), Is.True);
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
                );

            var factory = new PredicateFactory();
            var comparer = factory.Get(info);
            Assert.That(comparer.Compare(x,y), Is.True);
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
            Assert.Throws<ArgumentException>(delegate { factory.Get(info); });
        }
    }
}
