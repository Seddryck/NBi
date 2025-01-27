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
public class SentinelDateTimeLoopStrategyTest
{
    [Test]
    [TestCase(1, 5)]
    [TestCase(2, 5)]
    [TestCase(3, 4)]
    public void Run_day_CorrectResult(int stepDay, int expected)
    {
        var strategy = new SentinelCloseDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2018, 1, 5), new FixedDuration(new TimeSpan(stepDay, 0, 0, 0)));
        var final = new DateTime(2018, 1, 1);
        while (strategy.IsOngoing())
            final = strategy.GetNext();
        Assert.That(final, Is.EqualTo(new DateTime(2018, 1, expected)));
    }

    [Test]
    [TestCase(1, 5)]
    [TestCase(2, 5)]
    [TestCase(3, 4)]
    public void Run_Month_CorrectResult(int stepMonth, int expected)
    {
        var strategy = new SentinelCloseDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2018, 5, 10), new MonthDuration(stepMonth));
        var final = DateTime.MinValue;
        while (strategy.IsOngoing())
            final = strategy.GetNext();
        Assert.That(final, Is.EqualTo(new DateTime(2018, expected, 1)));
    }

    [Test]
    [TestCase(1, 2019)]
    [TestCase(2, 2018)]
    public void Run_Year_CorrectResult(int stepMonth, int expected)
    {
        var strategy = new SentinelCloseDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2019, 4, 10), new YearDuration(stepMonth));
        var final = DateTime.MinValue;
        while (strategy.IsOngoing())
            final = strategy.GetNext();
        Assert.That(final, Is.EqualTo(new DateTime(expected, 1, 1)));
    }

    [Test]
    public void GetNext_FirstTime_Seed()
    {
        var strategy = new SentinelCloseDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2018, 1, 2), new FixedDuration(new TimeSpan(1, 0, 0, 0)));
        Assert.That(strategy.GetNext(), Is.EqualTo(new DateTime(2018, 1, 1)));
    }

    [Test]
    public void IsOngoing_ZeroTimes_False()
    {
        var strategy = new SentinelCloseDateTimeLoopStrategy(new DateTime(2018, 1, 3), new DateTime(2018, 1, 2), new FixedDuration(new TimeSpan(1, 0, 0, 0)));
        Assert.That(strategy.IsOngoing(), Is.False);
    }

    [Test]
    public void IsOngoing_OneTimes_TrueThenFalse()
    {
        var strategy = new SentinelCloseDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new FixedDuration(new TimeSpan(1, 0, 0, 0)));
        Assert.That(strategy.IsOngoing(), Is.True);
        strategy.GetNext();
        Assert.That(strategy.IsOngoing(), Is.False);
    }

    [Test]
    public void IsOngoing_NTimes_True()
    {
        var strategy = new SentinelCloseDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2018, 1, 10), new FixedDuration(new TimeSpan(1, 0, 0, 0)));
        Assert.That(strategy.IsOngoing(), Is.True);
    }
}
