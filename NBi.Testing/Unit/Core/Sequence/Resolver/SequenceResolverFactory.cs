using NBi.Core.Scalar.Duration;
using NBi.Core.Sequence.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Resolver
{
    [TestFixture]
    public class SequenceResolverFactoryTest
    {
        [Test]
        public void Instantiate_List_ListSequenceResolver()
        {
            var args = new ListSequenceResolverArgs(new List<object>() { new DateTime(2015, 1, 1), new DateTime(2016, 1, 1) });

            var factory = new SequenceResolverFactory(null);
            var resolver = factory.Instantiate<DateTime>(args);
            Assert.That(resolver, Is.TypeOf<ListSequenceResolver<DateTime>>());
        }

        [Test]
        public void Instantiate_CountLoopDecimal_LoopSequenceResolver()
        {
            var args = new CountLoopSequenceResolverArgs<decimal, decimal>(5, 10, 2);

            var factory = new SequenceResolverFactory(null);
            var resolver = factory.Instantiate<decimal>(args);
            Assert.That(resolver, Is.TypeOf<LoopSequenceResolver<decimal>>());
        }

        [Test]
        public void Instantiate_SentinelLoopDecimal_LoopSequenceResolver()
        {
            var args = new SentinelLoopSequenceResolverArgs<decimal, decimal>(5, 10, 2);

            var factory = new SequenceResolverFactory(null);
            var resolver = factory.Instantiate<decimal>(args);
            Assert.That(resolver, Is.TypeOf<LoopSequenceResolver<decimal>>());
        }

        [Test]
        public void Instantiate_CountLoopDateTime_LoopSequenceResolver()
        {
            var args = new CountLoopSequenceResolverArgs<DateTime, IDuration>(3, new DateTime(2018,3,1), new FixedDuration(new TimeSpan(1,0,0,0)));

            var factory = new SequenceResolverFactory(null);
            var resolver = factory.Instantiate<DateTime>(args);
            Assert.That(resolver, Is.TypeOf<LoopSequenceResolver<DateTime>>());
        }

        [Test]
        public void Instantiate_SentinelLoopDateTime_LoopSequenceResolver()
        {
            var args = new SentinelLoopSequenceResolverArgs<DateTime, IDuration>(new DateTime(2018, 1, 1), new DateTime(2018, 3, 1), new FixedDuration(new TimeSpan(1, 0, 0, 0)));

            var factory = new SequenceResolverFactory(null);
            var resolver = factory.Instantiate<DateTime>(args);
            Assert.That(resolver, Is.TypeOf<LoopSequenceResolver<DateTime>>());
        }
    }
}
