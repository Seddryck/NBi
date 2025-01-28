using System;
using System.Linq;
using NBi.Core;
using NBi.Core.Scalar.Comparer;
using NBi.Extensibility;
using NUnit.Framework;

namespace NBi.Core.Testing.Scalar.Comparer;

[TestFixture]
public class NumericComparerTest
{
    #region SetUp & TearDown
    //Called only at instance creation
    [OneTimeSetUp]
    public void SetupMethods()
    {

    }

    //Called only at instance destruction
    [OneTimeTearDown]
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
        var comparer = new NumericComparer();
        var result = comparer.Compare(new decimal(1), "2.0");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_1DecimalAnd1dot5Double_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(new decimal(1), 1.5, 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_1DecimalAnd2dot5Double_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(new decimal(1), 2.5, 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_1DecimalAnd1dot5String_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(new decimal(1), "1.5", 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_1DecimalAnd2dot5String_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(new decimal(1), "2.5", 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_1DecimalAndAny_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(new decimal(1), "(any)", 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_1DecimalAndValue_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(new decimal(1), "(value)", 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_NullAndAny_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(null, "(any)", 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_NullAndValue_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(null, "(value)", 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_NullAnd1Decimal_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(null, new decimal(1), 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_NullAndNullPlaceHolder_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(null, "(null)", 1, SideTolerance.Both);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_NonNumericAndAny_FormatException()
    {
        var comparer = new NumericComparer();
        Assert.Throws<NBiException>(delegate { comparer.Compare("string", "(any)", 1, SideTolerance.Both); });
    }

    [Test]
    public void Compare_TwelveToElevenWithAToleranceOFTenPercent_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(12, 11, "10%");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToElevenWithAToleranceOFTenPercent_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(12, 11, "5%");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_NonValidPercentageForTolerance_ArgumentException()
    {
        var comparer = new NumericComparer();
        Assert.Throws<ArgumentException>(delegate { comparer.Compare(12, 11, "1,1%"); });
    }

    [Test]
    public void Compare_TwelveToIntervalTenToFourteen_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(12, "[10;14]");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToIntervalTenToTwelveClosed_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("[10;12]", 12);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToIntervalTenToTwelveClosedWithSpaces_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(" [-10 ; 12  ] ", 12);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToIntervalTenToTwelveOpen_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("[10;12[", 12);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_TwelveToIntervalTwelveOpenToFourteen_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("]12;14]", 12);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_TwelveToIntervalTenHalfToFourteenHalf_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("]10.5;14.5]", 12);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToIntervalTenToInfinity_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("[10;+INF]", 12);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToIntervalNegativeInfinityToFourteen_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("[-inf;14]", 12);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToIntervalPositive_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("(positive)", 12);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToIntervalPositiveSymbol_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("(0+)", -12);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_TwelveToIntervalNegative_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("(-)", -12);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToIntervalGreaterOrEqualToEighteen_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("(>=18)", 12);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_TwelveToIntervalGreaterOrEqualToEighteenHalf_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("(>=18.5)", 12);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_TwelveToIntervalLessThanEighteen_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare("(<18)", 12);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_MinusTwelveToMinusElevenWithAToleranceOFFiftyPercent_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-12, -11, "50%");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_MinusTwelveToMinusElevenWithAToleranceOfOnePercent_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-12, -11, "1%");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_TwelveToElevenWithOnePercentAndMinTwo_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(12, 11, "1% (min 2)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToElevenWithOnePercentAndMinDotFive_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(12, 11, "1% (min 0.5)");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_TwelveToElevenWithTenPercentAndMinDotFive_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(12, 11, "10% (min 0.5)");
        Assert.That(result.AreEqual, Is.True);
    }
    [Test]
    public void Compare_MinusTwelveToMinusElevenWithOnePercentAndMinTwo_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-12, -11, "1% (min 2)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_MinusTwelveToMinusElevenWithOnePercentAndMinDotFive_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-12, -11, "1% (min 0.5)");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_MinusTwelveToMinusElevenWithTenPercentAndMinDotFive_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-12, -11, "10% (min 0.5)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToElevenWithHundredPercentAndMaxTwo_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(12, 11, "100% (max 2)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwelveToElevenWithHundredPercentAndMaxDotFive_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(12, 11, "100% (max 0.5)");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_TwelveToElevenWithTenPercentAndMaxOne_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(12, 11, "10% (max 1)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_FiveAndSevenWithMoreThanFiftyPercent_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(5, 7, "+50%");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_SevenAndFiveWithMoreThanFiftyPercent_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(7, 5, "+50%");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_FiveAndSevenWithLessThanFiftyPercent_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(5, 7, "-50%");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_SevenAndFiveWithLessThanFiftyPercent_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(7, 5, "-50%");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_MinusFiveAndSevenWithMoreThanFiftyPercent_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-5, -7, "+50%");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_MinusSevenAndThreeWithMoreThanFiftyPercent_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-5, -3, "+50%");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_MinusFiveAndThreeWithLessThanFiftyPercent_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-5, -3, "-50%");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_MinusSevenAndNineWithLessThanFiftyPercent_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-7, -9, "-50%");
        Assert.That(result.AreEqual, Is.True);
    }

    //One-sided absolute

    [Test]
    public void Compare_FiveAndSevenWithMoreThanFifty_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(5, 7, "+50");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_SevenAndFiveWithMoreThanFifty_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(7, 5, "+50");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_FiveAndSevenWithLessThanFifty_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(5, 7, "-50");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_SevenAndFiveWithLessThanFifty_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(7, 5, "-50");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_MinusFiveAndSevenWithMoreThanFifty_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-5, -7, "+50");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_MinusSevenAndThreeWithMoreThanFifty_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-5, -3, "+50");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_MinusFiveAndThreeWithLessThanFifty_False()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-5, -3, "-50");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_MinusSevenAndNineWithLessThanFifty_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(-7, -9, "-50");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_Rounding_True()
    {
        var comparer = new NumericComparer();
        var result = comparer.Compare(7.46, 7.47, new NumericRounding(0.1m, Rounding.RoundingStyle.Round));
        Assert.That(result.AreEqual, Is.True);
    }
}
