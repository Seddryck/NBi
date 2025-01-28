using Expressif.Values;
using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Transformation.Transformer;

[TestFixture]
public class NativeTransformerTest
{
    [Test]
    public void Execute_TextToLastCharsWithVariable_Valid()
    {
        var variables = new ContextVariables();
        variables.Set("length", () => new GlobalVariable(new LiteralScalarResolver<int>(6)).GetValue());
        var code = "text-to-last-chars(@length)";
        var provider = new NativeTransformer<string>(new ServiceLocator(), new Context(variables));
        provider.Initialize(code);

        var result = provider.Execute("123456789");
        Assert.That(result, Is.EqualTo("456789"));
    }

    [Test]
    [TestCase("")]
    [TestCase("\t")]
    [TestCase(" \t")]
    [TestCase(" ")]
    [TestCase("\r\n")]
    [TestCase("\r\n \t \r  ")]
    public void Execute_BlankToEmpty_Empty(string value)
    {
        var code = "blank-to-empty";
        var provider = new NativeTransformer<string>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo("(empty)"));
    }

    [Test]
    public void Execute_NotInitialized_InvalidOperation()
    {
        var provider = new NativeTransformer<string>(new ServiceLocator(), Context.None);

        Assert.Throws<InvalidOperationException>(delegate { provider.Execute(200); });
    }

    [Test]
    [TestCase("(null)", 0)]
    [TestCase("foo", 3)]
    public void Execute_ChainingTransformations_Valid(object value, decimal expected)
    {
        var code = "null-to-empty | text-to-length";
        var provider = new NativeTransformer<string>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase("(null)", 0)]
    [TestCase("foo", 3)]
    [Ignore("Expressif needs to handle the value in front of")]
    public void Execute_ChainingTransformationsStartingByValue_Valid(object value, decimal expected)
    {
        var code = "value | null-to-empty | text-to-length";
        var provider = new NativeTransformer<string>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, 10)]
    [TestCase(10.566, 10.566)]
    [TestCase(null, 0)]
    [TestCase("", 0)]
    [TestCase("(null)", 0)]
    public void Execute_NullToZero_Valid(object value, decimal expected)
    {
        var code = "null-to-zero";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10.1, 11)]
    [TestCase(11, 11)]
    [TestCase(10.5, 11)]
    [TestCase(10.7, 11)]
    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("(null)", null)]
    public void Execute_NumericToCeiling_Valid(object value, decimal expected)
    {
        var code = "numeric-to-ceiling";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        if (expected == 0)
            Assert.That(result, Is.Null);
        else
            Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10.1, 10)]
    [TestCase(11, 11)]
    [TestCase(10.5, 10)]
    [TestCase(10.7, 10)]
    public void Execute_NumericToFloor_Valid(object value, decimal expected)
    {
        var code = "numeric-to-floor";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10.1, 10)]
    [TestCase(11, 11)]
    [TestCase(10.5, 10)]
    [TestCase(10.7, 11)]
    public void Execute_NumericToInteger_Valid(object value, decimal expected)
    {
        var code = "numeric-to-integer";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10.158, 10.16)]
    [TestCase(11, 11)]
    [TestCase(10.153, 10.15)]
    public void Execute_NumericToRound_Valid(object value, decimal expected)
    {
        var code = "numeric-to-round(2)";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, 8, 12, 10)]
    [TestCase(10, 12, 16, 12)]
    [TestCase(10, 6, 9, 9)]
    public void Execute_NumericToClip_Valid(object value, object min, object max, decimal expected)
    {
        var code = $"numeric-to-clip({min}, {max})";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, 11)]
    [TestCase(-1, 0)]
    public void Execute_NumericToIncrement_Valid(object value, decimal expected)
    {
        var code = $"numeric-to-increment";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, 8, 18)]
    [TestCase(10, -12, -2)]
    [TestCase(10, 0, 10)]
    public void Execute_NumericToAdd_Valid(object value, object additional, decimal expected)
    {
        var code = $"numeric-to-add({additional})";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, 2, 1, 12)]
    [TestCase(10, 2, 0, 10)]
    [TestCase(10, 2, 3, 16)]
    [TestCase(10, 2, -1, 8)]
    public void Execute_NumericToAddTimes_Valid(object value, object additional, object times, decimal expected)
    {
        var code = $"numeric-to-add({additional}, {times})";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, 2, 20)]
    [TestCase(10, 0, 0)]
    [TestCase(-10, -2, 20)]
    [TestCase(10, -1, -10)]
    public void Execute_NumericToMultiply_Valid(object value, object multiplicator, decimal expected)
    {
        var code = $"numeric-to-multiply({multiplicator})";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, 2, 5)]
    [TestCase(10, 1, 10)]
    [TestCase(-10, -2, 5)]
    [TestCase(10, -1, -10)]
    public void Execute_NumericToDivide_Valid(object value, object multiplicator, decimal expected)
    {
        var code = $"numeric-to-divide({multiplicator})";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(1, 1)]
    [TestCase(10, 0.1)]
    [TestCase(0.5, 2)]
    public void Execute_NumericToInvert_Valid(object value, decimal expected)
    {
        var code = $"numeric-to-invert";
        var provider = new NativeTransformer<decimal>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase("2019-03-11", "2019-03-11")]
    [TestCase("2019-02-11", "2019-03-01")]
    [TestCase("2019-04-11", "2019-03-31")]
    public void Execute_DateTimeToClip_Valid(object value, DateTime expected)
    {
        var code = $"dateTime-to-clip(2019-03-01, 2019-03-31)";
        var provider = new NativeTransformer<DateTime>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(value);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Execute_DateTimeToAddTimeSpan_Valid()
    {
        var code = $"dateTime-to-add(04:00:00, 4)";
        var provider = new NativeTransformer<DateTime>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(new DateTime(2017, 12, 31, 21, 0, 0));
        Assert.That(result, Is.EqualTo(new DateTime(2018, 01, 01, 13, 0, 0)));
    }

    [Test]
    public void Execute_DateTimeToAddTimeSpanWithoutTimes_Valid()
    {
        var code = $"dateTime-to-add(04:00:00)";
        var provider = new NativeTransformer<DateTime>(new ServiceLocator(), Context.None);
        provider.Initialize(code);

        var result = provider.Execute(new DateTime(2017, 12, 31, 21, 0, 0));
        Assert.That(result, Is.EqualTo(new DateTime(2018, 01, 01, 01, 0, 0)));
    }

    [Test]
    public void Execute_MultipleChains_Valid()
    {
        var code1 = $"path-to-filename-without-extension | text-to-datetime(yyyyMMdd_HHmmss) | local-to-utc(Brussels)";
        var provider1 = new NativeTransformer<string>(new ServiceLocator(), Context.None);
        provider1.Initialize(code1);

        var result1 = provider1.Execute("20191001_141542.xml");
        Assert.That(result1, Is.EqualTo(new DateTime(2019, 10, 01, 12, 15, 42)));

        var code2 = $"dateTime-to-floor-minute | dateTime-to-add(00:30:00, -1)";
        var provider2 = new NativeTransformer<DateTime>(new ServiceLocator(), Context.None);
        provider2.Initialize(code2);

        var result2 = provider2.Execute(result1);
        Assert.That(result2, Is.EqualTo(new DateTime(2019, 10, 01, 11, 45, 00)));
    }
}
