using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation.Transformer.Native;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Scalar.Resolver
{
    class ScalarResolverArgsFactoryTest
    {
        [Test]
        public void Instantiate_Literal_LiteralResolverArgs()
        {
            var factory = new ScalarResolverArgsFactory(new ServiceLocator(), null, string.Empty);
            var args = factory.Instantiate("First day of 2018 is a Monday");
            Assert.That(args, Is.TypeOf<LiteralScalarResolverArgs>());
        }

        [Test]
        public void Instantiate_Format_FormatResolverArgs()
        {
            var factory = new ScalarResolverArgsFactory(new ServiceLocator(), null, string.Empty);
            var args = factory.Instantiate("~First day of 2018 is a { @myVar: dddd}");
            Assert.That(args, Is.TypeOf<FormatScalarResolverArgs>());
        }

        [Test]
        public void Instantiate_Variable_GlobalVariableResolverArgs()
        {
            var factory = new ScalarResolverArgsFactory(new ServiceLocator(), null, string.Empty);
            var args = factory.Instantiate("@myVar");
            Assert.That(args, Is.TypeOf<GlobalVariableScalarResolverArgs>());
        }

        [Test]
        public void Instantiate_NativeTransformation_FunctionResolverArgs()
        {
            var factory = new ScalarResolverArgsFactory(new ServiceLocator(), null, string.Empty);
            var args = factory.Instantiate("@myVar | text-to-length");
            Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());
            var typedArgs = args as FunctionScalarResolverArgs;
            Assert.That(typedArgs.Resolver, Is.TypeOf<GlobalVariableScalarResolver<object>>());
            Assert.That(typedArgs.Transformations.Count, Is.EqualTo(1));
            Assert.That(typedArgs.Transformations.ElementAt(0), Is.TypeOf<TextToLength>());
        }

        [Test]
        public void Instantiate_NativeTransformationWithFormat_FunctionResolverArgs()
        {
            var factory = new ScalarResolverArgsFactory(new ServiceLocator(), null, string.Empty);
            var args = factory.Instantiate("~{@myVar : dddd} | text-to-length");
            Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());
            var typedArgs = args as FunctionScalarResolverArgs;
            Assert.That(typedArgs.Resolver, Is.TypeOf<FormatScalarResolver>());
            Assert.That(typedArgs.Transformations.Count, Is.EqualTo(1));
            Assert.That(typedArgs.Transformations.ElementAt(0), Is.TypeOf<TextToLength>());
        }

        [Test]
        public void Instantiate_NativeTransformationInsideFormat_FunctionResolverArgs()
        {
            var factory = new ScalarResolverArgsFactory(new ServiceLocator(), null, string.Empty);
            var args = factory.Instantiate("~{@myVar | dateTime-to-previous-month : dddd} ");
            Assert.That(args, Is.TypeOf<FormatScalarResolverArgs>());
        }
    }
}
