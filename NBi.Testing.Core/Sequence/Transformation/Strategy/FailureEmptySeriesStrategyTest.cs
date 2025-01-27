using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Transformation.Strategy;

public class FailureEmptySeriesStrategyTest
{
    [Test]
    public void Execute_Anyway_Exception()
    {
        var strategy = new FailureEmptySeriesStrategy();
        Assert.Throws<ArgumentNullException>(() => strategy.Execute());
    }
}
