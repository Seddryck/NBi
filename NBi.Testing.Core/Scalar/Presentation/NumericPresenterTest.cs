using NBi.Core.ResultSet;
using NBi.Core.Scalar.Presentation;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Presentation;

public class NumericPresenterTest
{
    [Test]
    [SetCulture("fr-fr")]
    [TestCase(10.50, "10.5")]
    [TestCase("10.50", "10.5")]
    public void Execute_NumericColumnObjectValueCultureSpecificWithFrFr_CorrectDisplay(object value, string expected)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(ColumnType.Numeric);
        var text = presenter.Execute(value);
        Assert.That(text, Is.EqualTo(expected));
    }

    [Test]
    [SetCulture("en-us")]
    [TestCase(10.50, "10.5")]
    [TestCase("10.50", "10.5")]
    public void Execute_TextColumnObjectValueCultureSpecificWithEnUs_CorrectDisplay(object value, string expected)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(ColumnType.Numeric);
        var text = presenter.Execute(value);
        Assert.That(text, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10, "10")]
    [TestCase(10.0f, "10")]
    [TestCase(10.40d, "10.4")]
    [TestCase("10.500", "10.5")]
    [TestCase("10.000", "10")]
    public void Execute_NumericColumnObjectValue_CorrectDisplay(object value, string expected)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(ColumnType.Numeric);
        var text = presenter.Execute(value);
        Assert.That(text, Is.EqualTo(expected));
    }

    public void Execute_NumericDecimal_CorrectDisplay()
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(ColumnType.Numeric);
        var text = presenter.Execute(10.400m);
        Assert.That(text, Is.EqualTo("10.4"));
    }
}
