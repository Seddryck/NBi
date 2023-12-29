using Expressif.Predicates.Numeric;
using Expressif.Predicates.Temporal;
using NBi.Core.Calculation.Asserting;
using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Sequence.Resolver
{
    [TestFixture]
    public class FilterSequenceResolverTest
    {
        [Test]
        public void Execute_Numeric_TwoElements()
        {
            var resolvers = new List<IScalarResolver>()
            {
                new LiteralScalarResolver<decimal>(10),
                new LiteralScalarResolver<decimal>(11),
                new LiteralScalarResolver<decimal>(12),
                new LiteralScalarResolver<decimal>(13),
            };
            var innerArgs = new ListSequenceResolverArgs(resolvers);
            var innerResolver = new ListSequenceResolver<decimal>(innerArgs);
            var predicate = new Predicate(new Modulo(() => 2, () => new LiteralScalarResolver<decimal>(0).Execute()!));
            var args = new FilterSequenceResolverArgs(innerResolver, predicate, null);

            var resolver = new FilterSequenceResolver<decimal>(args);
            var elements = resolver.Execute();
            Assert.That(elements.Count, Is.EqualTo(2));
            Assert.That(elements, Has.Member(10));
            Assert.That(elements, Has.Member(12));
        }

        [Test]
        public void Execute_DateTime_TwoElements()
        {
            var resolvers = new List<IScalarResolver>()
            {
                new LiteralScalarResolver<string>("2014-01-01"),
                new LiteralScalarResolver<string>("2015-01-01"),
                new LiteralScalarResolver<string>("2016-01-01"),
                new LiteralScalarResolver<string>("2017-01-01"),
            };
            var innerArgs = new ListSequenceResolverArgs(resolvers);
            var innerResolver = new ListSequenceResolver<DateTime>(innerArgs);
            var predicate = new Predicate(new After(() => new LiteralScalarResolver<DateTime>("2015-06-01").Execute()!));
            var transformation = new NativeTransformer<DateTime>(new ServiceLocator(), Context.None);
            transformation.Initialize("dateTime-to-next-year");
            var args = new FilterSequenceResolverArgs(innerResolver, predicate, transformation);

            var resolver = new FilterSequenceResolver<DateTime>(args);
            var elements = resolver.Execute();
            Assert.That(elements.Count, Is.EqualTo(3));
            Assert.That(elements, Has.Member(new DateTime(2015, 1, 1)));
            Assert.That(elements, Has.Member(new DateTime(2016, 1, 1)));
            Assert.That(elements, Has.Member(new DateTime(2017, 1, 1)));
        }
    }
}
