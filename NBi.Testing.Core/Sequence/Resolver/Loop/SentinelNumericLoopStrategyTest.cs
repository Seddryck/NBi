using NBi.Core.Sequence.Resolver.Loop;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Sequence.Resolver.Loop;

[TestFixture]
public class SentinelNumericLoopStrategyTest
{
    [Test]
    [TestCase(1, 5)]
    [TestCase(2, 5)]
    [TestCase(3, 4)]
    public void Run_parameters_CorrectResult(decimal step, decimal expected)
    {
        var strategy = new SentinelCloseNumericLoopStrategy(1, 5, step);
        var final = 0m;
        while (strategy.IsOngoing())
            final = strategy.GetNext();
        Assert.That(final, Is.EqualTo(expected));
    }

    [Test]
    public void GetNext_FirstTime_Seed()
    {
        var strategy = new SentinelCloseNumericLoopStrategy(10, 10, 1);
        Assert.That(strategy.GetNext(), Is.EqualTo(10));
    }

    [Test]
    public void IsOngoing_ZeroTimes_False()
    {
        var strategy = new SentinelCloseNumericLoopStrategy(10, 3, 2);
        Assert.That(strategy.IsOngoing(), Is.False);
    }

    [Test]
    public void IsOngoing_OneTimes_TrueThenFalse()
    {
        var strategy = new SentinelCloseNumericLoopStrategy(1, 1, 2);
        Assert.That(strategy.IsOngoing(), Is.True);
        strategy.GetNext();
        Assert.That(strategy.IsOngoing(), Is.False);
    }

    [Test]
    public void IsOngoing_NTimes_True()
    {
        var strategy = new SentinelCloseNumericLoopStrategy(1, 30, 2);
        Assert.That(strategy.IsOngoing(), Is.True);
    }
}
