using Moq;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Resolver;

public class GlobalVariableScalarResolverTest
{
    [Test]
    public void Execute_ExistingVariable_CorrectEvaluation()
    {
        var globalVariables = new Dictionary<string, IVariable>()
        {
            { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10*10"))) },
            { "otherVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
        };
        var args = new GlobalVariableScalarResolverArgs("myVar", new Context(globalVariables));
        var resolver = new GlobalVariableScalarResolver<int>(args);
        Assert.That(resolver.Execute(), Is.EqualTo(100));
    }

    [Test]
    public void Execute_ExistingVariableWrongType_CorrectEvaluation()
    {
        var globalVariables = new Dictionary<string, IVariable>()
        {
            { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("(10*10).ToString()"))) },
            { "otherVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
        };
        var args = new GlobalVariableScalarResolverArgs("myVar", new Context(globalVariables));
        var resolver = new GlobalVariableScalarResolver<int>(args);
        Assert.That(resolver.Execute(), Is.EqualTo(100));
    }

    [Test]
    public void Execute_ExistingVariableWrongTypeDateTime_CorrectEvaluation()
    {
        var globalVariables = new Dictionary<string, IVariable>()
        {
            { "myVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("\"2017-05-12\""))) },
            { "otherVar" , new GlobalVariable(new CSharpScalarResolver<object>( new CSharpScalarResolverArgs("10+10"))) }
        };
        var args = new GlobalVariableScalarResolverArgs("myVar", new Context(globalVariables));
        var resolver = new GlobalVariableScalarResolver<DateTime>(args);
        Assert.That(resolver.Execute(), Is.EqualTo(new DateTime(2017,5,12)));
    }

    [Test]
    public void Execute_ManyParallelExecutionOnlyOneEvaluation_CorrectEvaluation()
    {
        var resolverMock = Mock.Of<IScalarResolver>();
        Mock.Get(resolverMock).Setup(r => r.Execute()).Returns(true);

        var globalVariables = new Dictionary<string, IVariable>()
        {
            { "myVar" , new GlobalVariable(resolverMock) }
        };
        var args = new GlobalVariableScalarResolverArgs("myVar", new Context(globalVariables));
        var resolver = new GlobalVariableScalarResolver<bool>(args);
        Parallel.Invoke(
            () => resolver.Execute(),
            () => resolver.Execute()
        );
        Mock.Get(resolverMock).Verify(x => x.Execute(), Times.Once);
    }

    
}
