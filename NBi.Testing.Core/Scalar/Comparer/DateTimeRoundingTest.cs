using System;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;

namespace NBi.Core.Testing.Scalar.Comparer;

[TestFixture]
public class DateTimeRoundingTest
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
    [TestCase("2013-10-06", Rounding.RoundingStyle.Floor, "2013-10-06")]
    [TestCase("2013-10-06 06:00:00", Rounding.RoundingStyle.Floor, "2013-10-06")]
    [TestCase("2013-10-06 06:00:00", Rounding.RoundingStyle.Ceiling, "2013-10-07")]
    [TestCase("2013-10-06 06:00:00", Rounding.RoundingStyle.Round, "2013-10-06")]
    [TestCase("2013-10-06 18:00:00", Rounding.RoundingStyle.Round, "2013-10-07")]
   
    public void GetValue_ValueDayRoundingStyle_NewValue(DateTime value, Rounding.RoundingStyle roundingStyle, DateTime newValue)
    {
        var rounder = new DateTimeRounding(new TimeSpan(1,0,0,0), roundingStyle);
        
        Assert.That(rounder.GetValue(value), Is.EqualTo(newValue));
    }

    [Test]
    [TestCase("2013-10-06", Rounding.RoundingStyle.Floor, "2013-10-06")]
    [TestCase("2013-10-06 06:00:00", Rounding.RoundingStyle.Floor, "2013-10-06 06:00:00")]
    [TestCase("2013-10-06 06:45:00", Rounding.RoundingStyle.Floor, "2013-10-06 06:00:00")]
    [TestCase("2013-10-06 06:00:00", Rounding.RoundingStyle.Ceiling, "2013-10-06 06:00:00")]
    [TestCase("2013-10-06 06:15:00", Rounding.RoundingStyle.Ceiling, "2013-10-06 07:00:00")]
    [TestCase("2013-10-06 06:00:00", Rounding.RoundingStyle.Round, "2013-10-06 06:00:00")]
    [TestCase("2013-10-06 06:20:00", Rounding.RoundingStyle.Round, "2013-10-06 06:00:00")]
    [TestCase("2013-10-06 06:30:00", Rounding.RoundingStyle.Round, "2013-10-06 07:00:00")]
    [TestCase("2013-10-06 06:40:00", Rounding.RoundingStyle.Round, "2013-10-06 07:00:00")]
    public void GetValue_ValueHourRoundingStyle_NewValue(DateTime value, Rounding.RoundingStyle roundingStyle, DateTime newValue)
    {
        var rounder = new DateTimeRounding(new TimeSpan(0, 1, 0, 0), roundingStyle);

        Assert.That(rounder.GetValue(value), Is.EqualTo(newValue));
    }

    [Test]
    [TestCase("2013-10-06 06:10:00.526", Rounding.RoundingStyle.Floor, "2013-10-06 06:00:00")]
    [TestCase("2013-10-06 06:15:00.526", Rounding.RoundingStyle.Floor, "2013-10-06 06:15:00")]
    [TestCase("2013-10-06 06:15:00", Rounding.RoundingStyle.Floor, "2013-10-06 06:15:00")]
    [TestCase("2013-10-06 06:00:00", Rounding.RoundingStyle.Floor, "2013-10-06 06:00:00")]
    public void GetValue_ValueQuarterHourRoundingStyle_NewValue(DateTime value, Rounding.RoundingStyle roundingStyle, DateTime newValue)
    {
        var rounder = new DateTimeRounding(new TimeSpan(0, 0, 15, 0), roundingStyle);

        Assert.That(rounder.GetValue(value), Is.EqualTo(newValue));
    }
}
