using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Resolver;

class ScalarResolverArgsFactoryTest
{
    [Test]
    public void Instantiate_Literal_LiteralResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), Context.None);;
        var args = factory.Instantiate("First day of 2018 is a Monday");
        Assert.That(args, Is.TypeOf<LiteralScalarResolverArgs>());
    }

    [Test]
    public void Instantiate_Format_FormatResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), new Context());;
        var args = factory.Instantiate("~First day of 2018 is a { @myVar: dddd}");
        Assert.That(args, Is.TypeOf<FormatScalarResolverArgs>());
    }

    [Test]
    public void Instantiate_Variable_GlobalVariableResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), new Context());;
        var args = factory.Instantiate("@myVar");
        Assert.That(args, Is.TypeOf<GlobalVariableScalarResolverArgs>());
    }

    [Test]
    public void Instantiate_Variable_ContextResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), Context.None);;
        var args = factory.Instantiate("[myColumn]");
        Assert.That(args, Is.TypeOf<ContextScalarResolverArgs>());
    }

    [Test]
    public void Instantiate_NativeTransformation_FunctionResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), new Context());;
        var args = factory.Instantiate("@myVar | text-to-length");
        Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());
        var typedArgs = args as FunctionScalarResolverArgs;
        Assert.That(typedArgs!.Resolver, Is.TypeOf<GlobalVariableScalarResolver<object>>());
        Assert.That(typedArgs.Transformations.Count, Is.EqualTo(1));
        Assert.That(typedArgs.Transformations.ElementAt(0), Is.AssignableTo<INativeTransformation>());
    }

    [Test]
    public void Instantiate_NativeTransformationWithFormat_FunctionResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), new Context());;
        var args = factory.Instantiate("~{@myVar : dddd} | text-to-length");
        Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());
        var typedArgs = args as FunctionScalarResolverArgs;
        Assert.That(typedArgs!.Resolver, Is.TypeOf<FormatScalarResolver>());
        Assert.That(typedArgs.Transformations.Count, Is.EqualTo(1));
        Assert.That(typedArgs.Transformations.ElementAt(0), Is.AssignableTo<INativeTransformation>());
    }

    [Test]
    public void Instantiate_NativeTransformationInsideFormat_FormatResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), new Context());;
        var args = factory.Instantiate("~{@myVar | dateTime-to-previous-month : dddd} ");
        Assert.That(args, Is.TypeOf<FormatScalarResolverArgs>());
    }

    [Test]
    public void Instantiate_ContextWithFormat_LiteralResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), new Context()); ;
        var args = factory.Instantiate("[~{@date:yyyy}]");
        Assert.That(args, Is.TypeOf<ContextScalarResolverArgs>());
    }

    [Test]
    public void Instantiate_LiteralWithGravesAndPipes_LiteralResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), Context.None);;
        var args = factory.Instantiate("`a|b|c`");
        Assert.That(args, Is.TypeOf<LiteralScalarResolverArgs>());
        Assert.That(((LiteralScalarResolverArgs)args).Object, Is.EqualTo("a|b|c"));
    }

    [Test]
    public void Instantiate_LiteralWithGravesAndBrakets_LiteralResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), Context.None); ;
        var args = factory.Instantiate("`[a].[c]`");
        Assert.That(args, Is.TypeOf<LiteralScalarResolverArgs>());
        Assert.That(((LiteralScalarResolverArgs)args).Object, Is.EqualTo("[a].[c]"));
    }

    [Test]
    public void Instantiate_MDXParameter_LiteralResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), Context.None); ;
        var args = factory.Instantiate("[dimension].[hierarchy].[member]");
        Assert.That(args, Is.TypeOf<LiteralScalarResolverArgs>());
        Assert.That(((LiteralScalarResolverArgs)args).Object, Is.EqualTo("[dimension].[hierarchy].[member]"));
    }

    [Test]
    public void Instantiate_ColumnWithBrakets_ContextResolverArgs()
    {
        var factory = new ScalarResolverArgsFactory(new ServiceLocator(), Context.None); ;
        var args = factory.Instantiate("[[schema].[column]]");
        Assert.That(args, Is.TypeOf<ContextScalarResolverArgs>());
        Assert.That(((ContextScalarResolverArgs)args).ColumnIdentifier.Label, Is.EqualTo("[[schema].[column]]"));
    }
}
