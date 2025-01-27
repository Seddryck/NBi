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

public class BasePresenterTest
{
    [Test]
    [TestCase(ColumnType.Text)]
    [TestCase(ColumnType.Numeric)]
    [TestCase(ColumnType.DateTime)]
    [TestCase(ColumnType.Boolean)]
    public void Execute_NullValue_NullDisplay(ColumnType columnType)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(columnType);
        var text = presenter.Execute(null);
        Assert.That(text, Is.EqualTo("(null)"));
    }

    [Test]
    [TestCase(ColumnType.Text)]
    [TestCase(ColumnType.Numeric)]
    [TestCase(ColumnType.DateTime)]
    [TestCase(ColumnType.Boolean)]
    public void Execute_DBNullValue_NullDisplay(ColumnType columnType)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(columnType);
        var text = presenter.Execute(DBNull.Value);
        Assert.That(text, Is.EqualTo("(null)"));
    }

    [Test]
    [TestCase(ColumnType.Text)]
    [TestCase(ColumnType.Numeric)]
    [TestCase(ColumnType.DateTime)]
    [TestCase(ColumnType.Boolean)]
    public void Execute_StringNullValue_NullDisplay(ColumnType columnType)
    {
        var factory = new PresenterFactory();
        var presenter = factory.Instantiate(columnType);
        var text = presenter.Execute("(null)");
        Assert.That(text, Is.EqualTo("(null)"));
    }
}
