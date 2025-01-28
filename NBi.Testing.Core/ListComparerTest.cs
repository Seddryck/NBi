
using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NUnit.Framework;

namespace NBi.Core.Testing;

[TestFixture]
public class ListComparerTest
{
    [Test]
    public void Compare_Both_MissingUnexpectedNotNull()
    {
        var listComparer = new ListComparer();
        var res = listComparer.Compare(
            ["a", "b", "c"],
            ["a", "b", "c"],
            ListComparer.Comparison.Both
            );

        Assert.That(res.Missing, Is.Not.Null);
        Assert.That(res.Unexpected, Is.Not.Null);

    }

    [Test]
    public void Compare_Missing_MissingNotNullUnexpectedNull()
    {
        var listComparer = new ListComparer();
        var res = listComparer.Compare(
            ["a", "b", "c"],
            ["a", "b", "c"],
            ListComparer.Comparison.MissingItems
            );

        Assert.That(res.Missing, Is.Not.Null);
        Assert.That(res.Unexpected, Is.Null.Or.Empty);

    }

    [Test]
    public void Compare_Missing_MissingNullUnexpectedNotNull()
    {
        var listComparer = new ListComparer();
        var res = listComparer.Compare(
            ["a", "b", "c"],
            ["a", "b", "c"],
            ListComparer.Comparison.UnexpectedItems
            );

        Assert.That(res.Missing, Is.Null.Or.Empty);
        Assert.That(res.Unexpected, Is.Not.Null);

    }

    [Test]
    public void Compare_BothWithMissingAndUnexpectedItems_MissingUnexpectedNotNullAndNotEmpty()
    {
        var listComparer = new ListComparer();
        var res = listComparer.Compare(
            ["a", "b", "c"],
            ["a", "b", "d"],
            ListComparer.Comparison.Both
            );

        Assert.That(res.Missing, Is.Not.Null.And.Not.Empty);
        Assert.That(res.Unexpected, Is.Not.Null.And.Not.Empty);

    }

    [Test]
    public void Compare_MissingAndOneMissingItem_MissingHasCountOfOneWithCorrectMissingItem()
    {
        var listComparer = new ListComparer();
        var res = listComparer.Compare(
            ["a", "b", "z"],
            ["a", "b"],
            ListComparer.Comparison.Both
            );

        Assert.That(res.Missing, Has.Count.EqualTo(1));
        Assert.That(res.Missing.ElementAt(0), Is.EqualTo("z"));

    }


    [Test]
    public void Compare_UnexpectedAndOneUnexpectedItem_UnexpectedHasCountOfOneWithCorrectUnexpectedItem()
    {
        var listComparer = new ListComparer();
        var res = listComparer.Compare(
            ["a", "b"],
            ["a", "b", "z"],
            ListComparer.Comparison.Both
            );

        Assert.That(res.Unexpected, Has.Count.EqualTo(1));
        Assert.That(res.Unexpected.ElementAt(0), Is.EqualTo("z"));

    }

    [Test]
    public void Sample_LessThan10Items_DontThrowException()
    {
        var listComparer = new ListComparer();
        var res = listComparer.Compare(
            ["a", "b", "c", "d", "e", "f", "g", "h"],
            ["z"],
            ListComparer.Comparison.Both
            );

        var sampledRes = res.Sample(2);
        
        Assert.That(sampledRes.Missing.Count(), Is.EqualTo(2));
        Assert.That(sampledRes.MissingCount, Is.GreaterThan(2));

        Assert.That(sampledRes.Unexpected.Count(), Is.EqualTo(1));
        Assert.That(sampledRes.UnexpectedCount, Is.EqualTo(1));

    }
}
