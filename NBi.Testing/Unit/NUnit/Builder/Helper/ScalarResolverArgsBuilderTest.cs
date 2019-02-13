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
    }
}
