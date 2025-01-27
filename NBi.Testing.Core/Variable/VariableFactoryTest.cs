using Moq;
using NBi.Core.Injection;
using NBi.Core.Query.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NBi.Extensibility.Query;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Variable;

public class VariableFactoryTest
{
    [Test]
    public void Instantiate_CSharp_GlobalVariable()
    {
        var factory = new VariableFactory();
        var resolver = new CSharpScalarResolver<object>(new CSharpScalarResolverArgs("DateTime.Now.Year"));
        var variable = factory.Instantiate(VariableScope.Global, resolver);

        Assert.That(variable, Is.AssignableTo<IVariable>());
        Assert.That(variable, Is.AssignableTo<IRuntimeVariable>());
        Assert.That(variable, Is.TypeOf<GlobalVariable>());
    }

    [Test]
    public void Instantiate_QueryScalar_GlobalVariable()
    {
        var factory = new VariableFactory();
        var queryResolverArgsMock = new Mock<BaseQueryResolverArgs>(string.Empty, Array.Empty<IQueryParameter>(), Array.Empty<IQueryTemplateVariable>(), TimeSpan.Zero);
        var resolver = new QueryScalarResolver<object>(new QueryScalarResolverArgs(queryResolverArgsMock.Object), new ServiceLocator());
        var variable = factory.Instantiate(VariableScope.Global, resolver);

        Assert.That(variable, Is.AssignableTo<IVariable>());
        Assert.That(variable, Is.AssignableTo<IRuntimeVariable>());
        Assert.That(variable, Is.TypeOf<GlobalVariable>());
    }

    [Test]
    public void Instantiate_CSharp_IsNotEvaluated()
    {
        var factory = new VariableFactory();
        var resolver = new CSharpScalarResolver<object>(new CSharpScalarResolverArgs("DateTime.Now.Year"));
        var variable = factory.Instantiate(VariableScope.Global, resolver);

        Assert.That(variable.IsEvaluated, Is.False);
    }
}
