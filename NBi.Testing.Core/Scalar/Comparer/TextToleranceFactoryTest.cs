using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Comparer;

public class TextToleranceFactoryTest
{
    [Test]
    public void Instantiate_ExactNameDouble_Instantiated()
    {
        var textTolerance = new TextToleranceFactory().Instantiate("JaccardDistance(0.8)");
        Assert.That(textTolerance, Is.TypeOf<TextSingleMethodTolerance>());
        var tolerance = (TextSingleMethodTolerance)textTolerance;
        Assert.Multiple(() =>
        {
            Assert.That(tolerance.Style, Is.EqualTo("Jaccard distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8).Within(0.001));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        });
    }

    [Test]
    [TestCase("en-US")]
    [TestCase("fr-FR")]
    public void Instantiate_Decimal_Instantiated(string culture)
    {
        var currentCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
        var tolerance = new TextToleranceFactory().Instantiate("JaccardDistance(0.8)");
        Thread.CurrentThread.CurrentCulture = currentCulture;

        Assert.That(tolerance, Is.TypeOf<TextSingleMethodTolerance>());
        Assert.That(((TextSingleMethodTolerance)tolerance).Value, Is.EqualTo(0.8).Within(0.001));
    }

    [Test]
    public void Instantiate_ExactNameWithSpaceDouble_Instantiated()
    {
        var textTolerance = new TextToleranceFactory().Instantiate("jaccard disTance(0.8)");
        Assert.That(textTolerance, Is.TypeOf<TextSingleMethodTolerance>());
        var tolerance = (TextSingleMethodTolerance)textTolerance;
        Assert.Multiple(() =>
        {
            Assert.That(tolerance.Style, Is.EqualTo("Jaccard distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8).Within(0.001));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        });
    }

    [Test]
    public void Instantiate_ExactNameInt32_Instantiated()
    {
        var textTolerance = new TextToleranceFactory().Instantiate("LevenshteinDistance(0.8)");
        Assert.That(textTolerance, Is.TypeOf<TextSingleMethodTolerance>());
        var tolerance = (TextSingleMethodTolerance)textTolerance;
        Assert.Multiple(() =>
        {
            Assert.That(tolerance.Style, Is.EqualTo("Levenshtein distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8).Within(0.001));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        });
    }

    [Test]
    public void Instantiate_StartsWithNameInt32_Instantiated()
    {
        var textTolerance = new TextToleranceFactory().Instantiate("Hamming(0.8)");
        Assert.That(textTolerance, Is.TypeOf<TextSingleMethodTolerance>());
        var tolerance = (TextSingleMethodTolerance)textTolerance;
        Assert.Multiple(() =>
        {
            Assert.That(tolerance.Style, Is.EqualTo("Hamming distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8).Within(0.001));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        });
    }

    [Test]
    public void Instantiate_StartsWithCasingNameInt32_Instantiated()
    {
        var textTolerance = new TextToleranceFactory().Instantiate("hamming(0.8)");
        Assert.That(textTolerance, Is.TypeOf<TextSingleMethodTolerance>());

        var tolerance = (TextSingleMethodTolerance)textTolerance;
        Assert.Multiple(() =>
        {
            Assert.That(tolerance.Style, Is.EqualTo("Hamming distance"));
            Assert.That(tolerance.Value, Is.EqualTo(0.8).Within(0.001));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("alpha", "alpha"), Is.EqualTo(0));
        });
    }

    [Test]
    public void Instantiate_TwoMethodsAbbreviatedNames_Instantiated()
    {
        var textTolerance = new TextToleranceFactory().Instantiate("Overlap, Jaro-Winkler (weak )");
        Assert.That(textTolerance, Is.TypeOf<TextMultipleMethodsTolerance>());
        var tolerance = (TextMultipleMethodsTolerance)textTolerance;
        Assert.Multiple(() =>
        {
            Assert.That(tolerance.Style, Is.EqualTo("UseOverlapCoefficient, UseJaroWinklerDistance"));
            Assert.That(tolerance.Value, Is.EqualTo("Weak"));
            Assert.That(tolerance.Implementation, Is.Not.Null);
            Assert.That(tolerance.Implementation("je t'aime", "je t'eme"), Is.True);
        });
    }
}
