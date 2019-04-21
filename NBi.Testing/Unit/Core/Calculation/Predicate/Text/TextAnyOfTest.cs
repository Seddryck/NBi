using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Calculation.Predicate.Text
{
    public class TextAnyOfTest
    {
        [Test]
        [TestCase("Paris", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCulture)]
        [TestCase("paris", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCultureIgnoreCase)]
        public void Compare_Text_Success(object value, IScalarResolver reference, StringComparison stringComparison)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(i => i.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(i => i.ComparerType).Returns(ComparerType.AnyOf);
            predicate.As<IReferencePredicateInfo>().SetupGet(i => i.Reference).Returns(reference);
            predicate.As<ICaseSensitivePredicateInfo>().SetupGet(i => i.StringComparison).Returns(stringComparison);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Execute(value), Is.True);
        }

        [Test]
        [TestCase("Madrid", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCulture)]
        [TestCase("Pari", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCulture)]
        [TestCase("paris", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCulture)]
        public void Compare_Text_Failure(object value, object reference, StringComparison stringComparison)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(i => i.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(i => i.ComparerType).Returns(ComparerType.AnyOf);
            var resolver = new LiteralScalarResolver<string>(reference);
            predicate.As<IReferencePredicateInfo>().SetupGet(i => i.Reference).Returns(resolver);
            predicate.As<ICaseSensitivePredicateInfo>().SetupGet(i => i.StringComparison).Returns(stringComparison);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Execute(value), Is.False);
        }
    }
}
