using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation.Function;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Transformation.Aggregation.Numeric;

public class MaxTest
{
    [Test]
    public void Execute_Numeric_CorrectValue()
    {
        var list = new List<object>() { 1, 3m, 5d};
        var aggregation = new MaxNumeric();
        Assert.That(aggregation.Execute(list), Is.EqualTo(5));
    }

    [Test]
    public void Execute_DateTime_CorrectValue()
    {
        var list = new List<object>() { new DateTime(2010,1,1), new DateTime(2016,1,1), new DateTime(2019, 1, 1) };
        var aggregation = new MaxDateTime();
        Assert.That(aggregation.Execute(list), Is.EqualTo(new DateTime(2019, 1, 1)));
    }
}
