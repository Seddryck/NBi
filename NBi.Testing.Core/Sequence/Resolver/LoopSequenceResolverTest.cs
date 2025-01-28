using NBi.Core.Scalar.Duration;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Sequence.Resolver.Loop;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Sequence.Resolver;

[TestFixture]
public class LoopSequenceResolverTest
{
    [Test]
    public void Execute_CountNumeric_ExactSequence()
    {
        var args = new CountNumericLoopStrategy(5, 1, 1);
        var resolver = new LoopSequenceResolver<decimal>(args);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(5));
        Assert.That(elements, Is.EqualTo(new List<decimal>() { 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void Execute_CountDateTime_ExactSequence()
    {
        var args = new CountDateTimeLoopStrategy(3, new DateTime(2018, 1, 30), new FixedDuration(new TimeSpan(1, 0, 0, 0)));
        var resolver = new LoopSequenceResolver<DateTime>(args);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(3));
        Assert.That(elements, Is.EqualTo(new List<DateTime>() { new DateTime(2018, 1, 30), new DateTime(2018, 1, 31), new DateTime(2018, 2, 1) }));
    }

    [Test]
    public void Execute_SentinelNumeric_ExactSequence()
    {
        var args = new SentinelCloseNumericLoopStrategy(1, 5, 2);
        var resolver = new LoopSequenceResolver<decimal>(args);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(3));
        Assert.That(elements, Is.EqualTo(new List<decimal>() { 1, 3, 5 }));
    }

    [Test]
    public void Execute_SentinelDateTime_ExactSequence()
    {
        var args = new SentinelCloseDateTimeLoopStrategy(new DateTime(2018, 1, 28), new DateTime(2018, 2, 2), new FixedDuration(new TimeSpan(2, 0, 0, 0)));
        var resolver = new LoopSequenceResolver<DateTime>(args);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(3));
        Assert.That(elements, Is.EqualTo(new List<DateTime>() { new DateTime(2018, 1, 28), new DateTime(2018, 1, 30), new DateTime(2018, 2, 1) }));
    }

    [Test]
    public void Execute_ZeroCountNumeric_ExactSequence()
    {
        var args = new CountNumericLoopStrategy(0, 1, 1);
        var resolver = new LoopSequenceResolver<decimal>(args);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(0));
        Assert.That(elements, Is.EqualTo(new List<decimal>()));
    }

    [Test]
    public void Execute_SeedGreaterThanTerminalSentinelNumeric_ExactSequence()
    {
        var args = new SentinelCloseNumericLoopStrategy(10, 5, 2);
        var resolver = new LoopSequenceResolver<decimal>(args);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(0));
        Assert.That(elements, Is.EqualTo(new List<decimal>() { }));
    }

    [Test]
    public void Execute_SeedEqualToTerminalSentinelNumeric_ExactSequence()
    {
        var args = new SentinelCloseNumericLoopStrategy(10, 10, 2);
        var resolver = new LoopSequenceResolver<decimal>(args);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(1));
        Assert.That(elements, Is.EqualTo(new List<decimal>() { 10 }));
    }

    [Test]
    public void Execute_SentinelHalfOpenNumeric_ExactSequence()
    {
        var args = new SentinelHalfOpenNumericLoopStrategy(1, 7, 2);
        var resolver = new LoopSequenceResolver<decimal>(args);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(3));
        Assert.That(elements, Is.EqualTo(new List<decimal>() { 1, 3, 5}));
    }

    [Test]
    public void Execute_SentinelHalfOpenDateTime_ExactSequence()
    {
        var args = new SentinelHalfOpenDateTimeLoopStrategy(new DateTime(2018, 1, 28), new DateTime(2018, 2, 3), new FixedDuration(new TimeSpan(2, 0, 0, 0)));
        var resolver = new LoopSequenceResolver<DateTime>(args);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(3));
        Assert.That(elements, Is.EqualTo(new List<DateTime>() { new DateTime(2018, 1, 28), new DateTime(2018, 1, 30), new DateTime(2018, 2, 1) }));
    }
}
