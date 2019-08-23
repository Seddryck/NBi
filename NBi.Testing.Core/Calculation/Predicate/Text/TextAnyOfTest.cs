using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Calculation.Predicate.Text
{
    public class TextAnyOfTest
    {
        [Test]
        [TestCase("Paris", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCulture)]
        [TestCase("paris", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCultureIgnoreCase)]
        public void Compare_Text_Success(object value, IEnumerable<string> reference, StringComparison stringComparison)
        {
            var predicate = new Mock<CaseSensitivePredicateArgs>();
            predicate.SetupGet(i => i.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(i => i.ComparerType).Returns(ComparerType.AnyOf);
            var resolver = new ListSequenceResolver<string>(reference);
            predicate.SetupGet(i => i.Reference).Returns(resolver);
            predicate.SetupGet(i => i.StringComparison).Returns(stringComparison);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Execute(value), Is.True);
        }

        [Test]
        [TestCase("Madrid", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCulture)]
        [TestCase("Pari", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCulture)]
        [TestCase("paris", new[] { "Paris", "Bruxelles", "Amsterdam" }, StringComparison.InvariantCulture)]
        public void Compare_Text_Failure(object value, IEnumerable<string> reference, StringComparison stringComparison)
        {
            var predicate = new Mock<CaseSensitivePredicateArgs>();
            predicate.SetupGet(i => i.ColumnType).Returns(ColumnType.Text);
            predicate.SetupGet(i => i.ComparerType).Returns(ComparerType.AnyOf);
            var resolver = new ListSequenceResolver<string>(reference);
            predicate.SetupGet(i => i.Reference).Returns(resolver);
            predicate.SetupGet(i => i.StringComparison).Returns(stringComparison);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Execute(value), Is.False);
        }
    }
}
