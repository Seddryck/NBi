using NBi.Core.Calculation.Asserting;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Duplication;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;

namespace NBi.Core.Testing.ResultSet.Alteration.Duplication
{
    public class DuplicationFactoryTest
    {
        [Test]
        public void Instantiate_DuplicateArgs_DuplicateEngine()
        {
            var factory = new DuplicationFactory(ServiceLocator.None, Context.None);
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
