using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Asserting;
using NBi.Core.ResultSet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Calculation.Asserting;

public class CultureSensitivePredicateMatchesTest
{
    [Test]
    [TestCase(ComparerType.MatchesNumeric, "121")]
    [TestCase(ComparerType.MatchesNumeric, "1.21")]
    [TestCase(ComparerType.MatchesNumeric, "1000.21")]
    [TestCase(ComparerType.MatchesDate, "2016-12-25")]
    [TestCase(ComparerType.MatchesTime, "08:40:12")]
    [TestCase(ComparerType.MatchesDateTime, "2016-12-25 08:40:12")]
    public void Compare_TextWithoutCulture_Success(ComparerType comparerType, object x)
    {
        var predicate = new Mock<CultureSensitivePredicateArgs>(string.Empty);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Culture).Returns(string.Empty);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [Test]
    [TestCase(ComparerType.MatchesNumeric, "A.1")]
    [TestCase(ComparerType.MatchesNumeric, "A121")]
    [TestCase(ComparerType.MatchesDate, "25/12/2016")]
    [TestCase(ComparerType.MatchesDate, "12-25-2016")]
    [TestCase(ComparerType.MatchesDate, "206-12-25")]
    [TestCase(ComparerType.MatchesDate, "2016-12-25 07:42:00")]
    [TestCase(ComparerType.MatchesTime, "2016-12-25 07:42:00")]
    [TestCase(ComparerType.MatchesTime, "08:40")]
    [TestCase(ComparerType.MatchesTime, "08:40 AM")]
    [TestCase(ComparerType.MatchesDateTime, "25/12/2015 08:40PM")]
    public void Compare_TextWithoutCulture_Failure(ComparerType comparerType, object x)
    {
        var predicate = new Mock<CultureSensitivePredicateArgs>(string.Empty);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Culture).Returns(string.Empty);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.False);
    }

    [Test]
    [TestCase(ComparerType.MatchesNumeric, "121", "fr-fr")]
    [TestCase(ComparerType.MatchesNumeric, "1,21", "fr-fr")]
    [TestCase(ComparerType.MatchesNumeric, "1000,21", "fr-fr")]
    [TestCase(ComparerType.MatchesDate, "25/12/2016", "fr-fr")]
    [TestCase(ComparerType.MatchesDate, "05/12/2016", "fr-fr")]
    [TestCase(ComparerType.MatchesDate, "5/12/2016", "nl-be")]
    [TestCase(ComparerType.MatchesDateTime, "25/12/2015 08:40:16", "fr-fr")]
    [TestCase(ComparerType.MatchesNumeric, "121", "en-us")]
    [TestCase(ComparerType.MatchesNumeric, "1.21", "ja-jp")]
    [TestCase(ComparerType.MatchesNumeric, "1000.21", "ja-jp")]
    public void Compare_TextWithCulture_Success(ComparerType comparerType, object x, string culture)
    {
        var predicate = new Mock<CultureSensitivePredicateArgs>(culture);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Culture).Returns(culture);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [Test]
    [TestCase(ComparerType.MatchesNumeric, "A.1", "fr-fr")]
    [TestCase(ComparerType.MatchesNumeric, "1.21", "fr-fr")]
    [TestCase(ComparerType.MatchesNumeric, "A.1", "en-us")]
    [TestCase(ComparerType.MatchesNumeric, "1,211", "ja-jp")]
    [TestCase(ComparerType.MatchesDate, "12/25/2016", "fr-fr")]
    [TestCase(ComparerType.MatchesDate, "5/12/2016", "fr-fr")]
    [TestCase(ComparerType.MatchesDateTime, "25/12/2015 08:40:16", "ja-jp")]
    public void Compare_TextWithCulture_Failure(ComparerType comparerType, object x, string culture)
    {
        var predicate = new Mock<CultureSensitivePredicateArgs>(culture);
        predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Text);
        predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
        predicate.SetupGet(p => p.Culture).Returns(culture);

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(predicate.Object);
        Assert.That(comparer.Execute(x), Is.False);
    }

}
