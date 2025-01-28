using System;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;

namespace NBi.Core.Testing.Scalar.Comparer;

[TestFixture]
public class TextComparerTest
{
    #region SetUp & TearDown
    //Called only at instance creation
    [OneTimeSetUp]
    public void SetupMethods()
    {

    }

    //Called only at instance destruction
    [OneTimeTearDown]
    public void TearDownMethods()
    {
    }

    //Called before each test
    [SetUp]
    public void SetupTest()
    {
    }

    //Called after each test
    [TearDown]
    public void TearDownTest()
    {
    }
    #endregion

    [Test]
    public void Compare_StringAndSameString_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare("string", "string");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_StringAndOtherString_False()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare("string", "other string");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_StringAndSameStringUppercase_False()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare("string", "STRING");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_StringAndSubstring_False()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare("string", "str");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_StringAndAny_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare("string", "(any)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_StringAndValue_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare("string", "(value)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_NullAndAny_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(null, "(any)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_NullAndValue_False()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(null, "(value)");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_NullAndString_False()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(null, "string");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_NullAndNullPlaceHolder_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(null, "(null)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_EmptyAndEmptyPlaceHolder_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(string.Empty, "(empty)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_NonEmptyAndEmptyPlaceHolder_False()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare("string", "(empty)");
        Assert.That(result.AreEqual, Is.False);
    }


    [Test]
    public void Compare_NullAndEmptyPlaceHolder_False()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(null, "(empty)");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_1AndEmptyPlaceHolder_False()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(1, "(empty)");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_1AndBlankPlaceHolder_False()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(1, "(blank)");
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    public void Compare_TabAndBlankPlaceHolder_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare("\t", "(blank)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_TwoSpacesAndBlankPlaceHolder_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare("  ", "(blank)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_EmptyAndBlankPlaceHolder_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(string.Empty, "(blank)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    public void Compare_NullAndBlankPlaceHolder_True()
    {
        var comparer = new TextComparer();
        var result = comparer.Compare(null, "(blank)");
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    [TestCase("Hamming(0)")]
    [TestCase("Jaccard(0)")]
    [TestCase("Levenshtein(0)")]
    [TestCase("Overlap(0.65)")]
    public void Compare_EqualWord_True(string def)
    {
        var tolerance = new TextToleranceFactory().Instantiate(def);
        var comparer = new TextComparer();
        var result = comparer.Compare("Seddryck", "Seddryck", tolerance);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    [TestCase("Hamming(1)")]
    [TestCase("Jaccard(0.5)")]
    [TestCase("Jaro(0.5)")]
    [TestCase("Jaro-Winkler(0.5)")]
    [TestCase("Levenshtein(2)")]
    [TestCase("SorensenDice(0.5)")]
    [TestCase("Overlap(0.6)")]
    public void Compare_SmallDifference_True(string def)
    {
        var tolerance = new TextToleranceFactory().Instantiate(def);
        var comparer = new TextComparer();
        var result = comparer.Compare("Seddryck", "Xeddryck", tolerance);
        Assert.That(result.AreEqual, Is.True);
    }

    [Test]
    [TestCase("Hamming(2)")]
    [TestCase("Jaccard(0.4)")]
    [TestCase("Jaro(0.04)")]
    [TestCase("Jaro-Winkler(0.05)")]
    [TestCase("Levenshtein(2)")]
    [TestCase("SorensenDice(0.4)")]
    [TestCase("Overlap(0.7)")]
    public void Compare_LargeDifference_False(string def)
    {
        var tolerance = new TextToleranceFactory().Instantiate(def);
        var comparer = new TextComparer();
        var result = comparer.Compare("Seddryck", "Undefined", tolerance);
        Assert.That(result.AreEqual, Is.False);
    }

    [Test]
    [TestCase("ignore-case")]
    [TestCase(" IGnOre-Case ")]
    public void Compare_IgnoreCase_true(string def)
    {
        var tolerance = new TextToleranceFactory().Instantiate(def);
        var comparer = new TextComparer();
        var result = comparer.Compare("Seddryck", "seddryck", tolerance);
        Assert.That(result.AreEqual, Is.True);
    }
}
