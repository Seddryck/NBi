using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Comparer
{
    public class TextToleranceFactoryTest
    {
        [Test]
        public void Instantiate_ExactNameDouble_Instantiated()
        {
            var tolerance = new TextToleranceFactory().Instantiate("JaccardDistance(0.8)");
            Assert.That(tolerance, Is.TypeOf<TextTolerance>());
            Assert.That(tolerance.Style, Is.EqualTo("Jaccard distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        }

        [Test]
        public void Instantiate_ExactNameWithSpaceDouble_Instantiated()
        {
            var tolerance = new TextToleranceFactory().Instantiate("jaccard disTance(0.8)");
            Assert.That(tolerance, Is.TypeOf<TextTolerance>());
            Assert.That(tolerance.Style, Is.EqualTo("Jaccard distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        }

        [Test]
        public void Instantiate_ExactNameInt32_Instantiated()
        {
            var tolerance = new TextToleranceFactory().Instantiate("LevenshteinDistance(0.8)");
            Assert.That(tolerance, Is.TypeOf<TextTolerance>());
            Assert.That(tolerance.Style, Is.EqualTo("Levenshtein distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        }

        [Test]
        public void Instantiate_StartsWithNameInt32_Instantiated()
        {
            var tolerance = new TextToleranceFactory().Instantiate("Hamming(0.8)");
            Assert.That(tolerance, Is.TypeOf<TextTolerance>());
            Assert.That(tolerance.Style, Is.EqualTo("Hamming distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        }

        [Test]
        public void Instantiate_StartsWithCasingNameInt32_Instantiated()
        {
            var tolerance = new TextToleranceFactory().Instantiate("hamming(0.8)");
            Assert.That(tolerance, Is.TypeOf<TextTolerance>());
            Assert.That(tolerance.Style, Is.EqualTo("Hamming distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        }

    }
}
