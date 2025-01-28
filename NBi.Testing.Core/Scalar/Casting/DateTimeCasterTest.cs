using System;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using NBi.Core.Scalar.Casting;
using NBi.Core;
using NBi.Extensibility;

namespace NBi.Core.Testing.Scalar.Caster;

[TestFixture]
public class DateTimeCasterTest
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
    [TestCase("2013-10-16")]    //  yyyy-mm-dd
    [TestCase("16/10/2013")]    //  dd/mm/yyyy
    [TestCase("10/16/2013")]    //  mm/dd/yyyy
    [TestCase("10/11/2013")]    //  ambiguous mm or dd/mm or dd/yyyy
    [TestCase("10/16/13")]      //  mm/dd/yy
    [TestCase("5.12.78")]       //  d.m.yy
    [TestCase("10.5.2013")]     //  d.m.yyyy
    [TestCase("16-12")]         //  dd.mm
    [TestCase("12/17")]         //  mm/dd
    [TestCase("2020-09-08T13:42:29.190855+00:00")]      // RCF3339
    [TestCase("2020-09-08T13:42:29.190855Z")]           // RCF3339
    [TestCase("2020-09-08T13:42:29Z")]                  // RCF3339
    [TestCase("2020-09-08T13:42:29.190855-05:00")]      // RCF3339
    [TestCase("2020-09-08 13:42:29.190855+00:00")]      // close to RCF3339 but with a space rather than T
    [TestCase("2020-09-08 13:42:29.190855Z")]           // close to RCF3339 but with a space rather than T
    [TestCase("2020-09-08 13:42:29Z")]                  // close to RCF3339 but with a space rather than T
    [TestCase("2020-09-08 13:42:29.190855-05:00")]      // close to RCF3339 but with a space rather than T
    [TestCase("2020-09-08T13:42:29.190855Z")]           // close to RCF3339 but no timezone offset specified
    [TestCase("2020-09-08 13:42:29.190855")]            // close to RCF3339 but uses a space and no timezone offset
    public void IsValidDateTime_FromStringRepresentation_True(string value)
        => Assert.That(new DateTimeCaster().IsValid(value), Is.True);


    [Test]
    [TestCase("2013-10-16", 2013, 10, 16)]    //  yyyy-mm-dd
    [TestCase("16/10/2013", 2013, 10, 16)]    //  dd/mm/yyyy
    [TestCase("10/16/2013", 2013, 10, 16)]    //  mm/dd/yyyy
    [TestCase("10/11/2013", 2013, 10, 11)]    //  ambiguous mm or dd/mm or dd/yyyy
    [TestCase("10/16/13",   2013, 10, 16)]    //  mm/dd/yy
    [TestCase("5.12.78",    1978, 05, 12)]    //  m.d.yy
    [TestCase("10.5.2013",  2013, 10, 05)]    //  m.d.yyyy
    
    public void Execute_FromStringDateRepresentation_True(string value, int year, int month, int day)
        => Assert.That(new DateTimeCaster().Execute(value), Is.EqualTo(new DateTime(year, month, day)));

    [TestCase("2020-09-08T13:42:29.190+00:00"   , 1599572549190)]      // RCF3339
    [TestCase("2020-09-08 13:42:29.190+00:00"   , 1599572549190)]      // close to RCF3339 but with a space rather than T
    [TestCase("2020-09-08T13:42:29.190Z"        , 1599572549190)]      // RCF3339
    [TestCase("2020-09-08 13:42:29.190Z"        , 1599572549190)]      // close to RCF3339 but with a space rather than T
    [TestCase("2020-09-08T13:42:29.190"         , 1599572549190)]      // close to RCF3339 but no timezone offset specified
    [TestCase("2020-09-08 13:42:29.190"         , 1599572549190)]      // close to RCF3339 but uses a space and no timezone offset
    [TestCase("2020-09-08T13:42:29.190-05:00", 1599590549190)]      // RCF3339
    [TestCase("2020-09-08 13:42:29.190-05:00", 1599590549190)]      // close to RCF3339 but with a space rather than T
    [TestCase("2020-09-08T13:42:29Z", 1599572549000)]                  // RCF3339
    [TestCase("2020-09-08 13:42:29Z", 1599572549000)]                  // close to RCF3339 but with a space rather than T
    public void Execute_FromStringDateRepresentationRFC3339_True(string value, long ticks)
        => Assert.That(new DateTimeCaster().Execute(value), Is.EqualTo(DateTimeOffset.FromUnixTimeMilliseconds(ticks).DateTime));

    [Test]
    public void IsValidDateTime_String_False()
    {
        Assert.That(new DateTimeCaster().IsValid("DateTime"), Is.False);
    }

    [Test]
    public void Execute_NonDate_ThrowNBiException()
    {
        var ex = Assert.Throws<NBiException>(() => new DateTimeCaster().Execute("tomorrow"));
        Assert.That(ex!.Message, Is.EqualTo("Can't cast the value 'tomorrow' to a valid dateTime."));
    }

    [Test]
    public void Execute_DBNull_ThrowNBiException()
    {
        var ex = Assert.Throws<NBiException>(() => new DateTimeCaster().Execute(DBNull.Value));
        Assert.That(ex!.Message, Is.EqualTo("Can't cast the value '(null)' to a dateTime."));
    }

    [Test]
    public void Execute_Null_ThrowNBiException()
    {
        var ex = Assert.Throws<NBiException>(() => new DateTimeCaster().Execute(null));
        Assert.That(ex!.Message, Is.EqualTo("Can't cast the value '(null)' to a dateTime."));
    }


    [Test]
    public void Execute_NullString_ThrowNBiException()
    {
        var ex = Assert.Throws<NBiException>(() => new DateTimeCaster().Execute("(null)"));
        Assert.That(ex!.Message, Is.EqualTo("Can't cast the value '(null)' to a dateTime."));
    }
}
