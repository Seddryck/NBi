using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Testing.Sequence.Resolver.Resources;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Sequence.Resolver;

public class CustomSequenceResolverTest
{
    [Test]
    public void Execute_TypeWithoutParam_CorrectEvaluation()
    {
        var args = new CustomSequenceResolverArgs
        (
            new LiteralScalarResolver<string>(GetType().Assembly.Location),
            new LiteralScalarResolver<string>($"{typeof(MyCustomClass).Namespace}.{typeof(MyCustomClass).Name}"),
            new Dictionary<string, IScalarResolver>()
        );
        var resolver = new CustomSequenceResolver<string>(args);
        Assert.That(resolver.Execute(), Has.Member("myFirstValue"));
        Assert.That(resolver.Execute(), Has.Member("mySecondValue"));
        Assert.That(resolver.Execute(), Has.Member("myThirdValue"));
    }

    [Test]
    public void Execute_TypeWithParam_CorrectEvaluation()
    {
        var args = new CustomSequenceResolverArgs
        (
            new LiteralScalarResolver<string>(GetType().Assembly.Location),
            new LiteralScalarResolver<string>($"{typeof(MyCustomClassWithParams).Namespace}.{typeof(MyCustomClassWithParams).Name}"),
            new Dictionary<string, IScalarResolver>() { { "foo", new LiteralScalarResolver<int>(5) }, { "bar", new LiteralScalarResolver<DateTime>(new DateTime(2019, 1, 1)) } }
        );
        var resolver = new CustomSequenceResolver<DateTime>(args);
        var output = resolver.Execute();
        Assert.That(output, Has.Member(new DateTime(2019, 1, 6)));
        Assert.That(output, Has.Member(new DateTime(2018, 12, 27)));
    }
}
