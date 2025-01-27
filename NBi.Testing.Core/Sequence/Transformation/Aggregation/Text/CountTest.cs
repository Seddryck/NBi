using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Transformation.Aggregation.Function;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Transformation.Aggregation.Text;

public class CountTest
{
    [Test]
    public void Execute_Text_CorrectValue()
    {
        var list = new List<object>() { "alpha", "beta", "gamma" };
        var aggregation = new CountText();
        Assert.That(aggregation.Execute(list), Is.EqualTo(3));
    }

    [Test]
    public void Execute_EmptyValue_CorrectValue()
    {
        var list = new List<object>();
        var aggregation = new CountText();
        Assert.That(aggregation.Execute(list), Is.EqualTo(0));
    }
}
