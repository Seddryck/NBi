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
    [TestFixture]
    public class FunctionScalarResolverTest
    {
        [Test]
        public void Execute_ValueAndFunction_CorrectValue()
        {
            var args = new FunctionScalarResolverArgs(new LiteralScalarResolver<string>("abcd"), new[] { new TextToUpper() });
            var resolver = new FunctionScalarResolver<string>(args);
            Assert.That(resolver.Execute(), Is.EqualTo("ABCD"));
        }

        [Test]
        public void Execute_ValueAndTwoFunctions_CorrectValue()
        {
            var args = new FunctionScalarResolverArgs(
                new LiteralScalarResolver<string>(" abcd   ")
                , new INativeTransformation[] { new TextToTrim(), new TextToLength() }
            );
            var resolver = new FunctionScalarResolver<int>(args);
            Assert.That(resolver.Execute(), Is.EqualTo(4));
        }
    }
}
