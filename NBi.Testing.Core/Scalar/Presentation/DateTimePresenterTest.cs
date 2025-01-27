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

public class DateTimePresenterTest
{
    [Test]
    [TestCase("2010-07-04", "2010-07-04")]
    [TestCase("2010-07-04 00:00:00", "2010-07-04")]
    [TestCase("2010-07-04 11:30:10", "2010-07-04 11:30:10")]
    [TestCase("2010-07-04 11:30:10.000", "2010-07-04 11:30:10")]
    [TestCase("2010-07-04 11:30:10.325", "2010-07-04 11:30:10.325")]
    public void Execute_DateTimeColumnObjectValue_CorrectDisplay(object value, string expected)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(ColumnType.DateTime);
        var text = presenter.Execute(value);
        Assert.That(text, Is.EqualTo(expected));
    }

    private static IEnumerable DateTimeValues()
    {
        yield return new TestCaseData(new DateTime(2010, 7, 4), "2010-07-04");
        yield return new TestCaseData(new DateTime(2010, 7, 4, 11, 30, 10), "2010-07-04 11:30:10");
        yield return new TestCaseData(new DateTime(2010, 7, 4, 11, 30, 10, 325), "2010-07-04 11:30:10.325");
    }

    [Test]
    [TestCaseSource("DateTimeValues")]
    public void Execute_DateTimeColumnDateTimeValue_CorrectDisplay(object value, string expected)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(ColumnType.DateTime);
        var text = presenter.Execute(value);
        Assert.That(text, Is.EqualTo(expected));
    }
}
