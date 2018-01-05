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
        [TestCase(ComparerType.MatchesRegex, "Paris", "^[A-Z][a-z]+$")]
        public void Compare_Text_Success(ComparerType comparerType, object x, object y)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns(y);
            predicate.As<ICaseSensitivePredicateInfo>().SetupGet(p => p.StringComparison).Returns(StringComparison.InvariantCultureIgnoreCase);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Apply(x), Is.True);
        }

        [TestCase(ComparerType.StartsWith, "Paris", "p")]
        [TestCase(ComparerType.EndsWith, "Paris", "S")]
        [TestCase(ComparerType.Contains, "Paris", "AR")]
        [TestCase(ComparerType.MatchesRegex, "Paris", "^[A-Z]+$")]
        public void Compare_TextIgnoreCase_Success(ComparerType comparerType, object x, object y)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns(y);
            predicate.As<ICaseSensitivePredicateInfo>().SetupGet(p => p.StringComparison).Returns(StringComparison.InvariantCultureIgnoreCase);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
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
        [TestCase(ComparerType.MatchesRegex, "Paris", "^[A-Z]+$")]
        public void Compare_Text_Failure(ComparerType comparerType, object x, object y)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns(y);
            predicate.As<ICaseSensitivePredicateInfo>().SetupGet(p => p.StringComparison).Returns(StringComparison.InvariantCulture);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Apply(x), Is.False);
        }

        [Test]
        public void Compare_TextNull_Success()
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.Equal);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns("(null)");

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
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
        [TestCase(ComparerType.WithinRange, 1, "[1;2]")]
        [TestCase(ComparerType.WithinRange, 1, "(>0)")]
        [TestCase(ComparerType.WithinRange, -1, "(<=-1)")]
        public void Compare_Numeric_Success(ComparerType comparerType, object x, object y)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns(y);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Apply(x), Is.True);
        }

        [Test]
        [TestCase(ComparerType.Equal, 1, 1.5)]
        [TestCase(ComparerType.Equal, 1, "(null)")]
        [TestCase(ComparerType.LessThan, 1, -10)]
        [TestCase(ComparerType.LessThanOrEqual, 1, -10)]
        [TestCase(ComparerType.LessThanOrEqual, 1, "-10.0")]
        [TestCase(ComparerType.MoreThan, -10, 1)]
        [TestCase(ComparerType.MoreThanOrEqual, -10, 1)]
        [TestCase(ComparerType.MoreThanOrEqual, -1, 1)]
        [TestCase(ComparerType.MoreThanOrEqual, -1, "1.00")]
        [TestCase(ComparerType.WithinRange, 1, "]1;2]")]
        [TestCase(ComparerType.WithinRange, 1, "(<0)")]
        [TestCase(ComparerType.WithinRange, -1, "(>-1)")]
        public void Compare_Numeric_Failure(ComparerType comparerType, object x, object y)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns(y);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Apply(x), Is.False);
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
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.DateTime);
            predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns((object)new DateTime(2015, y, 1));

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Apply(new DateTime(2015, x, 1)), Is.True);
        }

        [Test]
        [TestCase("[2015-05-01;2016-05-01[")]
        public void Compare_DateTimeRange_Success(string range)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.DateTime);
            predicate.SetupGet(p => p.ComparerType).Returns(ComparerType.WithinRange);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns((object)range);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Apply(new DateTime(2015, 8, 1)), Is.True);
        }

        [Test]
        [TestCase(ComparerType.Equal, true, true)]
        [TestCase(ComparerType.Equal, "true", true)]
        [TestCase(ComparerType.Equal, "Yes", true)]
        public void Compare_Boolean_Success(ComparerType comparerType, object x, object y)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Boolean);
            predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns((object)y);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
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
            Assert.Throws<ArgumentOutOfRangeException>(delegate { factory.Instantiate(info); });
        }
    }
}
