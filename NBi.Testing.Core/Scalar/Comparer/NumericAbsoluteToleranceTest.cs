using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Comparer;

public class NumericAbsoluteToleranceTest
{
    [Test]
    public void ValueString_Fifty_Correct()
    {
        var tolerance = new NumericAbsoluteTolerance(new decimal(50), SideTolerance.Both);
        Assert.That(tolerance.ValueString, Is.EqualTo("50"));
    }

    [Test]
    public void ValueString_TwentyFivePercentDotSeven_Correct()
    {
        var tolerance = new NumericAbsoluteTolerance(new decimal(25.7), SideTolerance.Both);
        Assert.That(tolerance.ValueString, Is.EqualTo("25.7"));
    }
}
