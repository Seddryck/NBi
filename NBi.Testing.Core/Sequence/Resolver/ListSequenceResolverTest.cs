using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
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
    public class ListSequenceResolverTest
    {
        [Test]
        public void Execute_OneArg_OneElement()
        {
            var resolvers = new List<IScalarResolver>() { new LiteralScalarResolver<string>("1") };
            var args = new ListSequenceResolverArgs(resolvers);

            var resolver = new ListSequenceResolver<decimal>(args);
            var elements = resolver.Execute();
            Assert.That(elements.Count, Is.EqualTo(1));
            Assert.That(elements, Has.Member(1));
        }

        [Test]
        public void Execute_TwoArgs_TwoElements()
        {
            var resolvers = new List<IScalarResolver>()
            {
                new LiteralScalarResolver<string>("2015-01-01"),
                new LiteralScalarResolver<string>("2016-01-01"),
            };
            var args = new ListSequenceResolverArgs(resolvers);

            var resolver = new ListSequenceResolver<DateTime>(args);
            var elements = resolver.Execute();
            Assert.That(elements.Count, Is.EqualTo(2));
            Assert.That(elements, Has.Member(new DateTime(2015, 1, 1)));
            Assert.That(elements, Has.Member(new DateTime(2016, 1, 1)));
        }
    }
}
