﻿using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Transformation.Strategy;

public class ReplaceByDefaultStrategyTest
{
    [Test]
    public void Execute_NothingToReplace_NothingDropped()
    {
        var list = new List<object>() { 1, 3, 5 };
        var strategy = new ReplaceByDefaultStrategy(ColumnType.Numeric, 0);
        Assert.That(strategy.Execute(list).Count, Is.EqualTo(3));
    }

    [Test]
    public void Execute_NothingToReplace_SameValues()
    {
        var list = new List<object>() { 1, 3, 5 };
        var strategy = new ReplaceByDefaultStrategy(ColumnType.Numeric, 0);
        Assert.That(strategy.Execute(list), Has.Member(1));
        Assert.That(strategy.Execute(list), Has.Member(3));
        Assert.That(strategy.Execute(list), Has.Member(5));
    }

    [Test]
    public void Execute_Blank_BlankReplaced()
    {
        var list = new List<object>() { 1, "(blank)", 3, 5 };
        var strategy = new ReplaceByDefaultStrategy(ColumnType.Numeric, -1);
        Assert.That(strategy.Execute(list).Count, Is.EqualTo(4));
        Assert.That(strategy.Execute(list), Has.Member(-1));
        Assert.That(strategy.Execute(list), Has.Member(1));
        Assert.That(strategy.Execute(list), Has.Member(3));
        Assert.That(strategy.Execute(list), Has.Member(5));
    }

    [Test]
    public void Execute_Null_NullReplaced()
    {
        var list = new List<object?>() { 1, 3, 5, null };
        var strategy = new ReplaceByDefaultStrategy(ColumnType.Numeric, 0);
        Assert.That(strategy.Execute(list).Count, Is.EqualTo(4));
        Assert.That(strategy.Execute(list), Has.Member(0));
        Assert.That(strategy.Execute(list), Has.Member(1));
        Assert.That(strategy.Execute(list), Has.Member(3));
        Assert.That(strategy.Execute(list), Has.Member(5));
    }
}
