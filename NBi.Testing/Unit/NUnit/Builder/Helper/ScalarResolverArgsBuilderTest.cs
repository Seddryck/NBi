using NBi.Core.Scalar.Resolver;
using NBi.NUnit.Builder.Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Injection;
using NBi.Core.Transformation.Transformer.Native;

namespace NBi.Testing.Unit.NUnit.Builder.Helper
{
    public class ScalarResolverArgsBuilderTest
    {
        [Test]
        public void Build_VariableAndFunctions_FunctionScalarResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("@myVar | text-to-trim | text-to-length", null);
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());

            var typedArgs = args as FunctionScalarResolverArgs;
            Assert.That(typedArgs.Resolver, Is.TypeOf<GlobalVariableScalarResolver<object>>());
            Assert.That(typedArgs.Transformations, Has.Count.EqualTo(2));
            Assert.That(typedArgs.Transformations.ElementAt(0), Is.TypeOf<TextToTrim>());
            Assert.That(typedArgs.Transformations.ElementAt(1), Is.TypeOf<TextToLength>());
        }

        [Test]
        public void Build_Variable_GlobalVariableScalarResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("@myVar", null);
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<GlobalVariableScalarResolverArgs>());
        }

        [Test]
        public void Build_LiteralAndFunctions_FunctionScalarResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("2019-03-12 | dateTime-to-first-of-month", null);
            builder.Build();
            var args = builder.GetArgs();

            Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());
            var typedArgs = args as FunctionScalarResolverArgs;
            Assert.That(typedArgs.Resolver, Is.TypeOf<LiteralScalarResolver<object>>());
            Assert.That(typedArgs.Resolver.Execute(), Is.EqualTo("2019-03-12"));
            Assert.That(typedArgs.Transformations, Has.Count.EqualTo(1));
            Assert.That(typedArgs.Transformations.ElementAt(0), Is.TypeOf<DateTimeToFirstOfMonth>());
        }

        [Test]
        public void Build_Literal_LiteralResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("2019-03-12", null);
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<LiteralScalarResolverArgs>());
        }

        [Test]
        public void Build_FormatAndFunctions_FunctionScalarResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("~First day of 2018 is a { @myVar: dddd} | text-to-length", null);
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());

            var typedArgs = args as FunctionScalarResolverArgs;
            Assert.That(typedArgs.Resolver, Is.TypeOf<FormatScalarResolver>());
            Assert.That(typedArgs.Transformations, Has.Count.EqualTo(1));
            Assert.That(typedArgs.Transformations.ElementAt(0), Is.TypeOf<TextToLength>());
        }

        [Test]
        public void Build_FormatIncludingFunctions_FunctionScalarResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("~First day of 2018 is a { @myVar | dateTime-to-previous-month : dddd }", null);
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<FormatScalarResolverArgs>());
        }

        [Test]
        public void Build_Format_FormatResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("~First day of 2018 is a { @myVar: dddd}", null);
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<FormatScalarResolverArgs>());
        }
    }
}
