using NBi.Core.Sequence.Resolver.Loop;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Sequence.Resolver.Loop;

[TestFixture]
public class CountNumericLoopStrategyTest
{
    [Test]
    [TestCase(5, 0, 1, 4)]
    [TestCase(5, 1, 2, 9)]
    [TestCase(1, 3, 2, 3)]
    [TestCase(10, 3, 0, 3)]
    public void Run_parameters_CorrectResult(int count, decimal seed, decimal step, decimal expected)
    {
        var strategy = new CountNumericLoopStrategy(count, seed, step);
        var final = 0m;
        while (strategy.IsOngoing())
            final = strategy.GetNext();
        Assert.That(final, Is.EqualTo(expected));
    }

    [Test]
    public void GetNext_FirstTime_Seed()
    {
        var strategy = new CountNumericLoopStrategy(10, 4, 1);
        Assert.That(strategy.GetNext(), Is.EqualTo(4));
    }

    [Test]
    public void IsOngoing_ZeroTimes_False()
    {
        var strategy = new CountNumericLoopStrategy(0, 3, 2);
        Assert.That(strategy.IsOngoing(), Is.False);
    }

    [Test]
    public void IsOngoing_OneTimes_TrueThenFalse()
    {
        var strategy = new CountNumericLoopStrategy(1, 3, 2);
        Assert.That(strategy.IsOngoing(), Is.True);
        strategy.GetNext();
        Assert.That(strategy.IsOngoing(), Is.False);
    }

    [Test]
    public void IsOngoing_NTimes_True()
    {
        var strategy = new CountNumericLoopStrategy(10, 3, 2);
        Assert.That(strategy.IsOngoing(), Is.True);
    }
}
