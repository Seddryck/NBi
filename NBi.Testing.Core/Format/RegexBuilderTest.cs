using System;
using System.Data;
using System.Linq;
using Moq;
using NBi.Core.Format;
using NUnit.Framework;

namespace NBi.Core.Testing.Format;

[TestFixture]
public class RegexBuilderTest
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
    public void Build_NumericFormat_CorrectRegex()
    {
        var builder = new RegexBuilder();
        var result = builder.Build(
                Mock.Of<INumericFormat>
                (
                    x =>
                    x.DecimalDigits ==2
                    && x.DecimalSeparator=="."
                    && x.GroupSeparator==","
                )
            );

        Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\,?[0-9]{3})*\.[0-9]{2}$"));
        Assert.That("1,125,125.21", Does.Match(result));
    }

    [Test]
    public void Build_NumericFormatWithoutGroupSeparator_CorrectRegex()
    {
        var builder = new RegexBuilder();
        var result = builder.Build(
                Mock.Of<INumericFormat>
                (
                    x =>
                    x.DecimalDigits == 3
                    && x.DecimalSeparator == ","
                    && x.GroupSeparator == ""
                )
            );

        Assert.That(result, Is.EqualTo(@"^?[0-9]*\,[0-9]{3}$"));
        Assert.That("1125125,215", Does.Match(result));
    }

    [Test]
    public void Build_NumericFormatWithoutDecimal_CorrectRegex()
    {
        var builder = new RegexBuilder();
        var result = builder.Build(
                Mock.Of<INumericFormat>
                (
                    x =>
                    x.DecimalDigits == 0
                    && x.GroupSeparator == ""
                )
            );

        Assert.That(result, Is.EqualTo(@"^?[0-9]*$"));
        Assert.That("1125", Does.Match(result));
    }

    [Test]
    public void Build_NumericFormatWithoutDecimalByWithDefaultGroupSeparator_CorrectRegex()
    {
        var builder = new RegexBuilder();
        var result = builder.Build(
                Mock.Of<INumericFormat>
                (
                    x =>
                    x.DecimalDigits == 0
                    && x.DecimalSeparator == "."
                    && x.GroupSeparator == ","
                )
            );

        Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\,?[0-9]{3})*$"));
        Assert.That("1,125", Does.Match(result));
    }

    [Test]
    public void Build_CurrencyFormat_CorrectRegex()
    {
        var builder = new RegexBuilder();
        var result = builder.Build(
                Mock.Of<ICurrencyFormat>
                (
                    x =>
                    x.DecimalDigits == 2
                    && x.DecimalSeparator == "."
                    && x.GroupSeparator == ","
                    && x.CurrencySymbol == "$"
                    && x.CurrencyPattern==CurrencyPattern.Prefix
                )
            );

        Assert.That(result, Is.EqualTo(@"^\$?[0-9]{1,3}(?:\,?[0-9]{3})*\.[0-9]{2}$"));
        Assert.That("$1,125,125.21", Does.Match(result));
    }

    [Test]
    public void Build_CurrencyFormatSpaceEuro_CorrectRegex()
    {
        var builder = new RegexBuilder();
        var result = builder.Build(
                Mock.Of<ICurrencyFormat>
                (
                    x => 
                    x.DecimalDigits == 2
                    && x.DecimalSeparator == ","
                    && x.GroupSeparator == " "
                    && x.CurrencySymbol == "€"
                    && x.CurrencyPattern == CurrencyPattern.SuffixSpace
                )
            );

        Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\s?[0-9]{3})*\,[0-9]{2}\s\€$"));
        Assert.That("1 125 125,21 €", Does.Match(result));
    }

    [Test]
    public void Build_CurrencyFormatSpaceKiloEuro_CorrectRegex()
    {
        var builder = new RegexBuilder();
        var result = builder.Build(
                Mock.Of<ICurrencyFormat>
                (
                    x =>
                    x.DecimalDigits == 2
                    && x.DecimalSeparator == ","
                    && x.GroupSeparator == " "
                    && x.CurrencySymbol == "k€"
                    && x.CurrencyPattern == CurrencyPattern.SuffixSpace
                )
            );

        Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\s?[0-9]{3})*\,[0-9]{2}\sk\€$"));
        Assert.That("1 125 125,21 k€", Does.Match(result));
    }

    [Test]
    public void Build_CurrencyFormatSpaceKiloEuroinLetter_CorrectRegex()
    {
        var builder = new RegexBuilder();
        var result = builder.Build(
                Mock.Of<ICurrencyFormat>
                (
                    x => x.DecimalDigits == 2 &&
                    x.DecimalSeparator == "," &&
                    x.GroupSeparator == " " &&
                    x.CurrencySymbol == "Kilo Euro (k€)" &&
                    x.CurrencyPattern == CurrencyPattern.SuffixSpace
                )
            );

        Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\s?[0-9]{3})*\,[0-9]{2}\sKilo\sEuro\s\(k\€\)$"));
        Assert.That("1 125 125,21 Kilo Euro (k€)", Does.Match(result));
    }
}
