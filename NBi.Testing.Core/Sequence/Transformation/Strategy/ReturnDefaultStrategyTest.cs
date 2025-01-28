using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Transformation.Strategy;

public class ReturnDefaultStrategyTest
{
    [Test]
    public void Execute_Zero_ZeroReturned()
    {
        var strategy = new ReturnDefaultStrategy(0);
        Assert.That(strategy.Execute(), Is.EqualTo(0));
    }

    [Test]
    public void Execute_NothingToReplace_SameValues()
    {
        var strategy = new ReturnDefaultStrategy(-1);
        Assert.That(strategy.Execute(), Is.EqualTo(-1));
    }
}
