using NBi.Core.Scalar.Duration;
using NBi.Core.Sequence.Resolver.Loop;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Sequence.Resolver.Loop;

[TestFixture]
public class CountDateTimeLoopStrategyTest
{
    [Test]
    [TestCase(5, 1, 5)]
    [TestCase(5, 2, 9)]
    [TestCase(1, 2, 1)]
    [TestCase(10, 0, 1)]
    public void Run_parameters_CorrectResult(int count, int stepDay, int expected)
    {
        var strategy = new CountDateTimeLoopStrategy(count, new DateTime(2018, 1, 1), new FixedDuration(new TimeSpan(stepDay, 0, 0, 0)));
        var final = new DateTime(2018, 1, 1);
        while (strategy.IsOngoing())
            final = strategy.GetNext();
        Assert.That(final, Is.EqualTo(new DateTime(2018, 1, expected)));
    }

    [Test]
    [TestCase(1, 5, 2, 1, 9, 2018)]
    [TestCase(1, 5, 3, 1, 1, 2019)]
    [TestCase(31, 2, 3, 30, 4, 2018)]
    //[TestCase(31, 3, 3, 31, 7, 2018)]
    //[TestCase(31, 2, 1, 28, 2, 2018)]
    [TestCase(1, 13, 1, 1, 1, 2019)]
    [TestCase(1, 25, 1, 1, 1, 2020)]
    //[TestCase(31, 26, 1, 29, 2, 2020)]
    public void Run_MonthDuration_CorrectResult(int monthDay, int count, int stepMonth, int expectedDay, int expectedMonth, int expectedYear)
    {
        var strategy = new CountDateTimeLoopStrategy(count, new DateTime(2018, 1, monthDay), new MonthDuration(stepMonth));
        var final = DateTime.MinValue;
        while (strategy.IsOngoing())
            final = strategy.GetNext();
        Assert.That(final, Is.EqualTo(new DateTime(expectedYear, expectedMonth, expectedDay)));
    }

    [Test]
    [TestCase(5, 1, 2022)]
    [TestCase(2, 2, 2020)]
    public void Run_MonthDuration_CorrectResult(int count, int stepYear, int expectedYear)
    {
        var strategy = new CountDateTimeLoopStrategy(count, new DateTime(2018, 1, 1), new YearDuration(stepYear));
        var final = DateTime.MinValue;
        while (strategy.IsOngoing())
            final = strategy.GetNext();
        Assert.That(final, Is.EqualTo(new DateTime(expectedYear, 1, 1)));
    }

    [Test]
    public void GetNext_FirstTime_Seed()
    {
        var strategy = new CountDateTimeLoopStrategy(10, new DateTime(2018, 1, 1), new FixedDuration(new TimeSpan(1, 0, 0, 0)));
        Assert.That(strategy.GetNext(), Is.EqualTo(new DateTime(2018, 1, 1)));
    }

    [Test]
    public void IsOngoing_ZeroTimes_False()
    {
        var strategy = new CountDateTimeLoopStrategy(0, new DateTime(2015, 1, 1), new FixedDuration(new TimeSpan(1, 0, 0, 0)));
        Assert.That(strategy.IsOngoing(), Is.False);
    }

    [Test]
    public void IsOngoing_OneTimes_TrueThenFalse()
    {
        var strategy = new CountDateTimeLoopStrategy(1, new DateTime(2015, 1, 1), new FixedDuration(new TimeSpan(1, 0, 0, 0)));
        Assert.That(strategy.IsOngoing(), Is.True);
        strategy.GetNext();
        Assert.That(strategy.IsOngoing(), Is.False);
    }

    [Test]
    public void IsOngoing_NTimes_True()
    {
        var strategy = new CountDateTimeLoopStrategy(10, new DateTime(2015, 1, 1), new FixedDuration(new TimeSpan(1, 0, 0, 0)));
        Assert.That(strategy.IsOngoing(), Is.True);
    }
}
