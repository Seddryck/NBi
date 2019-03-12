using NBi.Core.Scalar.Resolver;
using NBi.NUnit.Builder.Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Injection;

namespace NBi.Testing.Unit.NUnit.Builder.Helper
{
    public class ScalarResolverArgsBuilderTest
    {
        [Test]
        public void Build_VariableAndFunctions_FunctionScalarResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("@myVar | text-to-trim | text-to-length");
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());
        }

        [Test]
        public void Build_Variable_GlobalVariableScalarResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("@myVar");
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<GlobalVariableScalarResolverArgs>());
        }

        [Test]
        public void Build_LiteralAndFunctions_FunctionScalarResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("2019-03-12 | dateTime-to-first-of-month");
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());
        }

        [Test]
        public void Build_Literal_LiteralResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("2019-03-12");
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<LiteralScalarResolverArgs>());
        }

        [Test]
        public void Build_FormatAndFunctions_FunctionScalarResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("~First day of 2018 is a { @myVar: dddd} | text-to-length");
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<FunctionScalarResolverArgs>());
        }

        [Test]
        public void Build_Format_FormatResolverArgs()
        {
            var builder = new ScalarResolverArgsBuilder(new ServiceLocator());
            builder.Setup("~First day of 2018 is a { @myVar: dddd}");
            builder.Build();
            var args = builder.GetArgs();
            Assert.That(args, Is.TypeOf<FormatScalarResolverArgs>());
        }
    }
}
