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
public class FormatTransformerTest
{
    [Test]
    public void Execute_Numeric_Format()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-us");
        var code = "0000.0000";
        var provider = new FormatTransformer<Decimal>();
        provider.Initialize(code);

        var result = provider.Execute(12.1);
        Assert.That(result, Is.EqualTo("0012.1000"));
    }

    [Test]
    public void Execute_DateTime_Format()
    {
        var code = "MM.yyyy";
        var provider = new FormatTransformer<DateTime>();
        provider.Initialize(code);

        var result = provider.Execute(new DateTime(2016,5,12));
        Assert.That(result, Is.EqualTo("05.2016"));

        result = provider.Execute(new DateTime(2016, 10, 12));
        Assert.That(result, Is.EqualTo("10.2016"));
    }

    [Test]
    public void Execute_NotInitialized_InvalidOperation()
    {
        var provider = new FormatTransformer<DateTime>();

        Assert.Throws<InvalidOperationException>(delegate { provider.Execute(200); });
    }
}
