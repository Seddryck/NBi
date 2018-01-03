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
    public class CultureSensitivePredicateMatchesTest
    {
        [Test]
        [TestCase(ComparerType.MatchesNumeric, "121")]
        [TestCase(ComparerType.MatchesNumeric, "1.21")]
        [TestCase(ComparerType.MatchesNumeric, "1000.21")]
        [TestCase(ComparerType.MatchesDate, "2016-12-25")]
        [TestCase(ComparerType.MatchesTime, "08:40")]
        public void Compare_Text_Success(ComparerType comparerType, object x)
        {
            var info = Mock.Of<ICultureSensitivePredicateInfo>(
                    i => i.ColumnType == ColumnType.Text
                    && i.ComparerType == comparerType
                );

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(info);
            Assert.That(comparer.Apply(x), Is.True);
        }

        [Test]
        [TestCase(ComparerType.MatchesNumeric, "A.1")]
        [TestCase(ComparerType.MatchesNumeric, "A121")]
        [TestCase(ComparerType.MatchesDate, "25/12/2016")]
        [TestCase(ComparerType.MatchesDate, "12-25-2016")]
        [TestCase(ComparerType.MatchesDate, "206-12-25")]
        [TestCase(ComparerType.MatchesDate, "2016-12-25 07:42:00")]
        [TestCase(ComparerType.MatchesTime, "2016-12-25 07:42:00")]
        [TestCase(ComparerType.MatchesTime, "08:40:12")]
        public void Compare_Text_Failure(ComparerType comparerType, object x)
        {
            var info = Mock.Of<ICultureSensitivePredicateInfo>(
                    i => i.ColumnType == ColumnType.Text
                    && i.ComparerType == comparerType
                );

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(info);
            Assert.That(comparer.Apply(x), Is.False);
        }

        [Test]
        [TestCase(ComparerType.MatchesNumeric, "121", "fr-fr")]
        [TestCase(ComparerType.MatchesNumeric, "1,21", "fr-fr")]
        [TestCase(ComparerType.MatchesNumeric, "1000,21", "fr-fr")]
        [TestCase(ComparerType.MatchesDate, "25/12/2016", "fr-fr")]
        [TestCase(ComparerType.MatchesDate, "05/12/2016", "fr-fr")]
        [TestCase(ComparerType.MatchesDate, "5/12/2016", "nl-be")]
        [TestCase(ComparerType.MatchesNumeric, "121", "en-us")]
        [TestCase(ComparerType.MatchesNumeric, "1.21", "en-us")]
        [TestCase(ComparerType.MatchesNumeric, "1000.21", "en-us")]
        public void Compare_Text_Success(ComparerType comparerType, object x, string culture)
        {
            var info = Mock.Of<ICultureSensitivePredicateInfo>(
                    i => i.ColumnType == ColumnType.Text
                    && i.ComparerType == comparerType
                    && i.Culture == culture
                );

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(info);
            Assert.That(comparer.Apply(x), Is.True);
        }

        [Test]
        [TestCase(ComparerType.MatchesNumeric, "A.1", "fr-fr")]
        [TestCase(ComparerType.MatchesNumeric, "1.21", "fr-fr")]
        [TestCase(ComparerType.MatchesNumeric, "A.1", "en-us")]
        [TestCase(ComparerType.MatchesNumeric, "1,211", "en-us")]
        [TestCase(ComparerType.MatchesDate, "12/25/2016", "fr-fr")]
        [TestCase(ComparerType.MatchesDate, "5/12/2016", "fr-fr")]
        public void Compare_Text_Failure(ComparerType comparerType, object x, string culture)
        {
            var info = Mock.Of<ICultureSensitivePredicateInfo>(
                    i => i.ColumnType == ColumnType.Text
                    && i.ComparerType == comparerType
                    && i.Culture == culture
                );

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(info);
            Assert.That(comparer.Apply(x), Is.False);
        }

    }
}
