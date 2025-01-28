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

public class BooleanPresenterTest
{
    [Test]
    [TestCase(true)]
    [TestCase("TRUE")]
    [TestCase("true")]
    [TestCase(1)]
    public void Execute_BooleanColumnObjectValueForTrue_DisplayIsTrue(object value)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(ColumnType.Boolean);
        var text = presenter.Execute(value);
        Assert.That(text, Is.EqualTo("True"));
    }

    [Test]
    [TestCase(false)]
    [TestCase("FALSE")]
    [TestCase("false")]
    [TestCase(0)]
    public void Execute_BooleanColumnObjectValueForFalse_DisplayIsFalse(object value)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(ColumnType.Boolean);
        var text = presenter.Execute(value);
        Assert.That(text, Is.EqualTo("False"));
    }
}
