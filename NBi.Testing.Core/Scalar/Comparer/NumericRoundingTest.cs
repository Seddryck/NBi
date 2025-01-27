using System;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;

namespace NBi.Core.Testing.Scalar.Comparer;

[TestFixture]
public class NumericRoundingTest
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
    [TestCase(105, 20, Rounding.RoundingStyle.Floor, 100)]
    [TestCase(105, 2, Rounding.RoundingStyle.Floor, 104)]
    [TestCase(105, 2, Rounding.RoundingStyle.Round, 106)]
    [TestCase(108, 10, Rounding.RoundingStyle.Round, 110)]
    [TestCase(105, 20, Rounding.RoundingStyle.Ceiling, 120)]
    [TestCase(105, 2, Rounding.RoundingStyle.Ceiling, 106)]
    [TestCase(105, 5, Rounding.RoundingStyle.Floor, 105)]
    [TestCase(105, 5, Rounding.RoundingStyle.Ceiling, 105)]
    [TestCase(105, 5, Rounding.RoundingStyle.Round, 105)]
    [TestCase(-105, 5, Rounding.RoundingStyle.Round, -105)]
    [TestCase(1.2345, 0.01, Rounding.RoundingStyle.Round, 1.23)]
    [TestCase(-1.2345, 0.01, Rounding.RoundingStyle.Round, -1.23)]
    [TestCase(-1.2355, 0.01, Rounding.RoundingStyle.Round, -1.24)]
    [TestCase(-1.2355, 0.01, Rounding.RoundingStyle.Floor, -1.24)]
    [TestCase(-1.2345, 0.01, Rounding.RoundingStyle.Ceiling, -1.23)]
    public void GetValue_ValueStepStyle_NewValue(decimal value, decimal step, Rounding.RoundingStyle roundingStyle, decimal newValue)
    {
        var rounder = new NumericRounding(step, roundingStyle);
        
        Assert.That(rounder.GetValue(value), Is.EqualTo(newValue));
    }

    [Test]
    [TestCase(19.10)]
    [TestCase(19.14)]
    [TestCase(19.15)]
    [TestCase(19.16)]
    [TestCase(0.01)]
    [TestCase(0)]
    [TestCase(-0.01)]
    [TestCase(-19.10)]
    [TestCase(-19.14)]
    [TestCase(-19.15)]
    [TestCase(-19.16)]
    public void GetValue_ValueStepStyle_EquivalentToMathRound(decimal value)
    {
        var rounder = new NumericRounding(0.1m, Rounding.RoundingStyle.Round);

        Assert.That(rounder.GetValue(value), Is.EqualTo(Math.Round(value, 1)));
    }

    [Test]
    [TestCase(19.1)]
    [TestCase(19.4)]
    [TestCase(19.5)]
    [TestCase(19.6)]
    [TestCase(0.1)]
    [TestCase(0)]
    [TestCase(-0.1)]
    [TestCase(-19.1)]
    [TestCase(-19.4)]
    [TestCase(-19.5)]
    [TestCase(-19.6)]
    public void GetValue_ValueStepStyle_EquivalentToMathCeiling(decimal value)
    {
        var rounder = new NumericRounding(1, Rounding.RoundingStyle.Ceiling);

        Assert.That(rounder.GetValue(value), Is.EqualTo(Math.Ceiling(value)));
    }

    [Test]
    [TestCase(19.1)]
    [TestCase(19.4)]
    [TestCase(19.5)]
    [TestCase(19.6)]
    [TestCase(0.1)]
    [TestCase(0)]
    [TestCase(-0.1)]
    [TestCase(-19.1)]
    [TestCase(-19.4)]
    [TestCase(-19.5)]
    [TestCase(-19.6)]
    public void GetValue_ValueStepStyle_EquivalentToMathFloor(decimal value)
    {
        var rounder = new NumericRounding(1, Rounding.RoundingStyle.Floor);

        Assert.That(rounder.GetValue(value), Is.EqualTo(Math.Floor(value)));
    }
}
