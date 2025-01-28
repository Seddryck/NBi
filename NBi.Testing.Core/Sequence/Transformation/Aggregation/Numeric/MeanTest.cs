using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation.Function;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Transformation.Aggregation.Numeric;

public class MeanTest
{
    [Test]
    public void Execute_Array_CorrectValue()
    {
        var list = new List<object>() { 1, 3, 5};
        var aggregation = new AverageNumeric();
        Assert.That(aggregation.Execute(list), Is.EqualTo(3));
    }
}
