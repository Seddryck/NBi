using Moq;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Testing.Scalar.Resolver.Resources;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Resolver;

public class CustomScalarResolverTest
{
    [Test]
    public void Execute_TypeWithoutParam_CorrectEvaluation()
    {
        var args = new CustomScalarResolverArgs
        (
            new LiteralScalarResolver<string>(GetType().Assembly.Location),
            new LiteralScalarResolver<string>($"{typeof(MyCustomClass).Namespace}.{typeof(MyCustomClass).Name}"),
            new Dictionary<string, IScalarResolver>()
        );
        var resolver = new CustomScalarResolver<string>(args);
        Assert.That(resolver.Execute(), Is.EqualTo("myValue"));
    }

    [Test]
    public void Execute_TypeWithParam_CorrectEvaluation()
    {
        var args = new CustomScalarResolverArgs
        (
            new LiteralScalarResolver<string>(GetType().Assembly.Location),
            new LiteralScalarResolver<string>($"{typeof(MyCustomClassWithParams).Namespace}.{typeof(MyCustomClassWithParams).Name}"),
            new Dictionary<string, IScalarResolver>() { { "foo", new LiteralScalarResolver<int>(5) }, { "bar", new LiteralScalarResolver<DateTime>(new DateTime(2019, 1, 1)) } }
        );
        var resolver = new CustomScalarResolver<DateTime>(args);
        var output = resolver.Execute();
        Assert.That(output, Is.EqualTo(new DateTime(2019,1,6)));
    }

    //[Test]
    //public void Execute_TwoCalls_OneExecution()
    //{
    //    var factory = new CustomScalarFactory();
    //    var mock = new Mock<ICustomScalarEvaluation>();
    //    mock.SetupSequence(x => x.Execute()).Returns(true).Returns(false);

    //    var customEvaluation = factory.Instantiate(mock.Object);
    //    Assert.That(customEvaluation.Execute(), Is.True);
    //    Assert.That(customEvaluation.Execute(), Is.True);
    //    mock.Verify(x => x.Execute(), Times.Once);
    //}
}
