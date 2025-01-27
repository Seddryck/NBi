using NBi.Core.Transformation.Transformer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Transformation.Transformer;

[TestFixture]
public class NCalcTransformerTest
{
    [Test]
    public void Execute_Numeric_Multiplied()
    {
        var code = "Sqrt(value) * 1.21";
        var provider = new NCalcTransformer<decimal>();
        provider.Initialize(code);

        var result = provider.Execute(100);
        Assert.That(result, Is.EqualTo(12.1));
    }

    [Test]
    public void Execute_NumericDouble_Multiplied()
    {
        var code = "value * 1.21";
        var provider = new NCalcTransformer<decimal>();
        provider.Initialize(code);

        var result = provider.Execute(10);
        Assert.That(result, Is.EqualTo(12.1));
    }

    public void Execute_NumericDecimal_Multiplied()
    {
        var code = "value * 1.21";
        var provider = new NCalcTransformer<decimal>();
        provider.Initialize(code);

        var result = provider.Execute(10m);
        Assert.That(result, Is.EqualTo(12.1));
    }

    [Test]
    public void Execute_String_Translated()
    {
        var code = "in (value , 'Oui', 'Yes', 'Ja')";
        var provider = new NCalcTransformer<string>();
        provider.Initialize(code);

        var result = provider.Execute("Oui");
        Assert.That(result, Is.EqualTo(true));

        result = provider.Execute("Non");
        Assert.That(result, Is.EqualTo(false));
    }
    
    [Test]
    public void Execute_MultipleString_Permuted()
    {
        var code = "value * 1.21";
        var provider = new NCalcTransformer<decimal>();
        provider.Initialize(code);

        Assert.That(provider.Execute(10), Is.EqualTo(12.1));
        Assert.That(provider.Execute(100), Is.EqualTo(121));
        Assert.That(provider.Execute(20), Is.EqualTo(24.2));
    }

    [Test]
    public void Execute_NotInitialized_InvalidOperation()
    {
        var provider = new NCalcTransformer<string>();

        Assert.Throws<InvalidOperationException>(delegate { provider.Execute(200); });
    }
}
