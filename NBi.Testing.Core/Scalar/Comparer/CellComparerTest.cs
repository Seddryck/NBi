using System;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using NBi.Core.ResultSet;

namespace NBi.Core.Testing.Scalar.Comparer;

[TestFixture]
public class CellComparerTest
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

    [TestCase(10, ColumnType.Numeric)]
    [TestCase(10.12212, ColumnType.Numeric)]
    [TestCase("alpha", ColumnType.Text)]
    [TestCase("2016-12-10", ColumnType.DateTime)]
    [TestCase(true, ColumnType.Boolean)]
    [Test]
    public void Compare_ToItself_True(object value, ColumnType columnType)
    {
        var comparer = new CellComparer();
        var result = comparer.Compare(value, value, columnType, null, null);
        Assert.That(result.AreEqual, Is.True);
    }

    [TestCase(10, ColumnType.Numeric)]
    [TestCase(10.12212, ColumnType.Numeric)]
    [TestCase("alpha", ColumnType.Text)]
    [TestCase("2016-12-10", ColumnType.DateTime)]
    [TestCase(true, ColumnType.Boolean)]
    [Test]
    public void Compare_ToAny_True(object value, ColumnType columnType)
    {
        var comparer = new CellComparer();
        var result = comparer.Compare(value, "(any)", columnType, null, null);
        Assert.That(result.AreEqual, Is.True);
    }

    [TestCase(10, ColumnType.Numeric)]
    [TestCase(10.12212, ColumnType.Numeric)]
    [TestCase("alpha", ColumnType.Text)]
    [TestCase("2016-12-10", ColumnType.DateTime)]
    [TestCase(true, ColumnType.Boolean)]
    [Test]
    public void Compare_ToValue_True(object value, ColumnType columnType)
    {
        var comparer = new CellComparer();
        var result = comparer.Compare(value, "(value)", columnType, null, null);
        Assert.That(result.AreEqual, Is.True);
    }

    [TestCase("(null)", "(blank)")]
    [TestCase("(blank)", "(null)")]
    [Test]
    public void Compare_BlankNullEmpty_True(object x, object y)
    {
        var comparer = new CellComparer();
        var result = comparer.Compare(x, y, ColumnType.Text, null, null);
        Assert.That(result.AreEqual, Is.True);
    }

}
