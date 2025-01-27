using System;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NUnit.Framework;

namespace NBi.Core.Testing.Query;

[TestFixture]
public class DbTypeBuilderTest
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
    public void Build_DateTime_DateTime()
    {
        var builder = new DbTypeBuilder();
        var result = builder.Build("DateTime");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(DbType.DateTime));
    }

    [Test]
    public void Build_Varchar_AnsiString()
    {
        var builder = new DbTypeBuilder();
        var result = builder.Build("Varchar");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(DbType.AnsiString));
    }

    [Test]
    public void Build_Varchar255_AnsiStringWithSize()
    {
        var builder = new DbTypeBuilder();
        var result = builder.Build("varchar(255)");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(DbType.AnsiString));
        Assert.That(result.Size, Is.EqualTo(255));
    }

    [Test]
    public void Build_Decimal10And2_DecimalWithSizeAndPrecision()
    {
        var builder = new DbTypeBuilder();
        var result = builder.Build("Decimal (10,2)");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(DbType.Decimal));
        Assert.That(result.Size, Is.EqualTo(10));
        Assert.That(result.Precision, Is.EqualTo(2));
    }
}
