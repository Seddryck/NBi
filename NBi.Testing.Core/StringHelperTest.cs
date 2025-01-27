using System;
using System.Linq;
using System.Text;
using NBi.Core;
using NUnit.Framework;

namespace NBi.Core.Testing;

[TestFixture]
public class StringHelperTest
{
    [Test]
    public void RemoveDiacritics_FrenchChars_NormalizedString()
    {
        var str = "àâäéèêëïöùüÿç";
        var strNormalized = str.RemoveDiacritics();

        Assert.That(strNormalized, Is.EqualTo("aaaeeeeiouuyc"));
    }

    [Test]
    public void LevenshteinDistance_SameWord_Zero()
    {
        var str = "alphabeta";
        var compared = "alphabeta";
        var distance = str.LevenshteinDistance(compared);

        Assert.That(distance, Is.EqualTo(0));
    }

    [Test]
    public void LevenshteinDistance_OneMissingLetter_One()
    {
        var str = "alphabeta";
        var compared = "alphabet";
        var distance = str.LevenshteinDistance(compared);

        Assert.That(distance, Is.EqualTo(1));
    }

    [Test]
    public void LevenshteinDistance_OneAdditionalLetter_One()
    {
        var str = "alphabeta";
        var compared = "alphabetax";
        var distance = str.LevenshteinDistance(compared);

        Assert.That(distance, Is.EqualTo(1));
    }

    [Test]
    public void LevenshteinDistance_TwoPermuttedLetters_Two()
    {
        var str = "alphabeta";
        var compared = "alpahbeta";
        var distance = str.LevenshteinDistance(compared);

        Assert.That(distance, Is.EqualTo(2));
    }

    [Test]
    public void LevenshteinDistance_TwoIncorrectLetters_Two()
    {
        var str = "alphabeta";
        var compared = "alpXXbeta";
        var distance = str.LevenshteinDistance(compared);

        Assert.That(distance, Is.EqualTo(2));
    }

    [Test]
    public void LevenshteinDistance_TwoMissingLetters_Two()
    {
        var str = "alphabeta";
        var compared = "alphbea";
        var distance = str.LevenshteinDistance(compared);

        Assert.That(distance, Is.EqualTo(2));
    }


    [Test]
    public void LevenshteinDistance_IncorrectLetterAfterMissing_Two()
    {
        var str = "alphabeta";
        var compared = "alphxxxx";
        var distance = str.LevenshteinDistance(compared);

        Assert.That(distance, Is.EqualTo(5));
    }


}
