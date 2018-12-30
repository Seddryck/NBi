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
    public class ListSequenceResolverTest
    {
        [Test]
        public void Execute_OneArg_OneElement()
        {
            var objects = new List<object>() { 1 };
            var args = new ListSequenceResolverArgs(objects);

            var resolver = new ListSequenceResolver<int>(args);
            var elements = resolver.Execute();
            Assert.That(elements.Count(), Is.EqualTo(1));
            Assert.That(elements, Has.Member(1));
        }

        [Test]
        public void Execute_TwoArgs_TwoElements()
        {
            var objects = new List<object>() { new DateTime(2015,1,1), new DateTime(2016,1,1) };
            var args = new ListSequenceResolverArgs(objects);

            var resolver = new ListSequenceResolver<DateTime>(args);
            var elements = resolver.Execute();
            Assert.That(elements.Count(), Is.EqualTo(2));
            Assert.That(elements, Has.Member(new DateTime(2015, 1, 1)));
            Assert.That(elements, Has.Member(new DateTime(2016, 1, 1)));
        }
    }
}
