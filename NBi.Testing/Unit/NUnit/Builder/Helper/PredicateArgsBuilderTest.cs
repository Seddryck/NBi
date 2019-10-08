using NBi.Core.Calculation.Predicate;
using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.NUnit.Builder.Helper;
using NBi.Xml.Constraints.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.NUnit.Builder.Helper
{
    public class PredicateArgsBuilderTest
    {
        [Test]
        public void Build_VariableAndFunctions_FunctionScalarResolverArgs()
        {
            var predicateXml = new MoreThanXml()
            {
                Reference = "#12 | text-to-upper | text-to-first-chars([ColA])"
            };

            var builder = new PredicateArgsBuilder(new ServiceLocator(), new Context(null));
            var args = builder.Execute(Core.ResultSet.ColumnType.Text, predicateXml);
            Assert.That(args, Is.AssignableTo<ReferencePredicateArgs>());

            var typedArgs = args as ReferencePredicateArgs;
            Assert.That(typedArgs.Reference, Is.TypeOf<FunctionScalarResolver<string>>());

            var function = typedArgs.Reference as FunctionScalarResolver<string>;
            Assert.That(function.Args.Resolver, Is.TypeOf<ContextScalarResolver<object>>());
        }
    }
}
