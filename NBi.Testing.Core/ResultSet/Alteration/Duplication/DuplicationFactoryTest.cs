using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predication;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Duplication;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Alteration.Duplication
{
    public class DuplicationFactoryTest
    {
        [Test]
        public void Instantiate_DuplicateArgs_DuplicateEngine()
        {
            var factory = new DuplicationFactory(null, new Context(null));
            var extender = factory.Instantiate(new DuplicateArgs(
                new PredicationFactory().Instantiate(new PredicateFactory().Instantiate(new PredicateArgs()), new ColumnOrdinalIdentifier(0)),
                new LiteralScalarResolver<int>(1),
                new List<OutputArgs>()
                ));
            Assert.That(extender, Is.Not.Null);
            Assert.That(extender, Is.TypeOf<DuplicateEngine>());
        }
    }
}
