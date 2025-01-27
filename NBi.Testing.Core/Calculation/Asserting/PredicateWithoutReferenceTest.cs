using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Asserting;
using NBi.Core.ResultSet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Calculation.Asserting;

public class PredicateWithoutReferenceTest
{
    [Test]
    [TestCase(ComparerType.Null, null)]
    [TestCase(ComparerType.Null, "(null)")]
    [TestCase(ComparerType.Empty, "")]
    [TestCase(ComparerType.Empty, "(empty)")]
    [TestCase(ComparerType.NullOrEmpty, null)]
    [TestCase(ComparerType.NullOrEmpty, "(null)")]
    [TestCase(ComparerType.NullOrEmpty, "")]
    [TestCase(ComparerType.NullOrEmpty, "(empty)")]
    [TestCase(ComparerType.LowerCase, "")]
    [TestCase(ComparerType.LowerCase, "(empty)")]
    [TestCase(ComparerType.LowerCase, "(null)")]
    [TestCase(ComparerType.LowerCase, "abcd1235")]
    [TestCase(ComparerType.UpperCase, "")]
    [TestCase(ComparerType.UpperCase, "(empty)")]
    [TestCase(ComparerType.UpperCase, "(null)")]
    [TestCase(ComparerType.UpperCase, "ABD1235")]
    public void Apply_Text_Success(ComparerType comparerType, object x)
    {
        var info = Mock.Of<PredicateArgs>(
                i => i.ColumnType== ColumnType.Text
                && i.ComparerType == comparerType
            );

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(info);
        Assert.That(comparer.Execute(x), Is.True);
    }

    [Test]
    [TestCase(ComparerType.Null, null)]
    [TestCase(ComparerType.Null, "(null)")]
    [TestCase(ComparerType.Empty, "")]
    [TestCase(ComparerType.Empty, "(empty)")]
    [TestCase(ComparerType.NullOrEmpty, null)]
    [TestCase(ComparerType.NullOrEmpty, "(null)")]
    [TestCase(ComparerType.NullOrEmpty, "")]
    [TestCase(ComparerType.NullOrEmpty, "(empty)")]
    [TestCase(ComparerType.LowerCase, "")]
    [TestCase(ComparerType.LowerCase, "(empty)")]
    [TestCase(ComparerType.LowerCase, "(null)")]
    [TestCase(ComparerType.LowerCase, "abcd1235")]
    [TestCase(ComparerType.UpperCase, "")]
    [TestCase(ComparerType.UpperCase, "(empty)")]
    [TestCase(ComparerType.UpperCase, "(null)")]
    [TestCase(ComparerType.UpperCase, "ABD1235")]
    public void Execute_NotText_Failure(ComparerType comparerType, object x)
    {
        var info = Mock.Of<PredicateArgs>(
                i => i.ColumnType == ColumnType.Text
                && i.Not == true
                && i.ComparerType == comparerType
            );

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(info);
        Assert.That(comparer.Execute(x), Is.False);
    }

    [Test]
    [TestCase(ComparerType.LowerCase, "abCD1235")]
    [TestCase(ComparerType.UpperCase, "Abc1235")]
    public void Apply_Text_Failure(ComparerType comparerType, object x)
    {
        var info = Mock.Of<PredicateArgs>(
                i => i.ColumnType == ColumnType.Text
                && i.ComparerType == comparerType
            );

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(info);
        Assert.That(comparer.Execute(x), Is.False);
    }

    [Test]
    [TestCase(ComparerType.Null, null, true)]
    [TestCase(ComparerType.Null, 1, false)]
    [TestCase(ComparerType.Integer, 1, true)]
    [TestCase(ComparerType.Integer, 1.0001, false)]
    public void Compare_Numeric_Result(ComparerType comparerType, object x, bool result)
    {
        var info = Mock.Of<PredicateArgs>(
                i => i.ColumnType == ColumnType.Numeric
                && i.ComparerType == comparerType
            );

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(info);
        Assert.That(comparer.Execute(x), Is.EqualTo(result));
    }

    [Test]
    public void Apply_NullDateTime_Success()
    {
        var info = Mock.Of<PredicateArgs>(
                i => i.ColumnType == ColumnType.DateTime
                && i.ComparerType == ComparerType.Null
            );

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(info);
        Assert.That(comparer.Execute(null), Is.True);
    }

    [Test]
    public void Compare_NotNullDateTime_Failure()
    {
        var info = Mock.Of<PredicateArgs>(
                i => i.ColumnType == ColumnType.DateTime
                && i.ComparerType == ComparerType.Null
            );

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(info);
        Assert.That(comparer.Execute(new DateTime(2015, 10, 1)), Is.False);
    }

    [Test]
    [TestCase(ComparerType.OnTheDay, 0, 0, 0, true)]
    [TestCase(ComparerType.OnTheDay, 0, 0, 1, false)]
    [TestCase(ComparerType.OnTheHour, 4, 0, 0, true)]
    [TestCase(ComparerType.OnTheHour, 4, 15, 0, false)]
    [TestCase(ComparerType.OnTheMinute, 3, 10, 0, true)]
    [TestCase(ComparerType.OnTheMinute, 3, 10, 11, false)]
    public void Compare_DateTime_Result(ComparerType comparerType, int hours, int minutes, int seconds, bool result)
    {
        var info = Mock.Of<PredicateArgs>(
                i => i.ColumnType == ColumnType.DateTime
                && i.ComparerType == comparerType
            );

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(info);
        Assert.That(comparer.Execute(new DateTime(2015, 10, 1, hours, minutes, seconds)), Is.EqualTo(result));
    }

    [Test]
    [TestCase(ComparerType.Null, null, true)]
    [TestCase(ComparerType.Null, "true", false)]
    [TestCase(ComparerType.Null, "(null)", true)]
    [TestCase(ComparerType.True, true, true)]
    [TestCase(ComparerType.True, false, false)]
    [TestCase(ComparerType.False, false, true)]
    [TestCase(ComparerType.False, true, false)]
    public void Compare_Boolean_Success(ComparerType comparerType, object x, bool result)
    {
        var info = Mock.Of<PredicateArgs>(
                i => i.ColumnType == ColumnType.Boolean
                && i.ComparerType == comparerType
            );

        var factory = new PredicateFactory();
        var comparer = factory.Instantiate(info);
        Assert.That(comparer.Execute(x), Is.EqualTo(result));
    }
    
}
