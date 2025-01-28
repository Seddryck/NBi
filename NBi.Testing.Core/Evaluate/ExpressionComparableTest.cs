using System;
using System.Linq;
using NBi.Core.Evaluate;
using NUnit.Framework;

namespace NBi.Core.Testing.Evaluate;

[TestFixture]
public class ExpressionComparableTest
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
    public void GetComparer_EqualSymbol_EqualFunction()
    {
        var expression = new ExpressionComparable("=4+4");
        expression.Parse();

        Assert.That(expression.Comparer?.Compare == expression.Comparer!.Equal);
    }

    [Test]
    public void GetComparer_NotEqualSymbol_NotEqualFunction()
    {
        var expression = new ExpressionComparable("!=4+4");
        expression.Parse();

        Assert.That(expression.Comparer?.Compare == expression.Comparer!.NotEqual);
    }

    [Test]
    public void Expression_SimpleMaths_ExpectedParsing()
    {
        var expression = new ExpressionComparable("!=4+4");
        expression.Parse();

        Assert.That(expression.Expression, Is.EqualTo("4+4"));
    }
}
