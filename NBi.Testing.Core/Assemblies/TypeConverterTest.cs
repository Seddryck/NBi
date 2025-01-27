#region Using directives
using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Assemblies;
using NUnit.Framework;

#endregion
namespace NBi.Core.Testing.Assemblies;

[TestFixture]
public class TypeConverterTest
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
    public void Convert_StringToString_CorrectValue()
    {
        //Build the SUT
        var tc = new TypeConverter();

        //Call the method to test
        var actual = tc.Convert("My God", typeof(string));

        //Assertion
        Assert.That(actual, Is.InstanceOf<string>());
        Assert.That(actual, Is.EqualTo("My God"));
    }
    
    [Test]
    public void Convert_StringToDecimal_CorrectValue()
    {
        //Build the SUT
        var tc = new TypeConverter();
        
        //Call the method to test
        var actual = tc.Convert("10", typeof(decimal));

        //Assertion
        Assert.That(actual, Is.InstanceOf<decimal>());
        Assert.That(actual, Is.EqualTo(10));
    }

    [Test]
    public void Convert_StringToEnum_CorrectValue()
    {
        //Build the SUT
        var tc = new TypeConverter();

        //Call the method to test
        var actual = tc.Convert("Beta", typeof(Resource.Enumeration));

        //Assertion
        Assert.That(actual, Is.InstanceOf<Resource.Enumeration>());
        Assert.That(actual, Is.EqualTo(Resource.Enumeration.Beta));
    }

    [Test]
    public void Convert_StringToDateTime_YMDCorrectValue()
    {
        //Build the SUT
        var tc = new TypeConverter();

        //Call the method to test
        var actual = tc.Convert("2012-05-10", typeof(DateTime));

        //Assertion
        Assert.That(actual, Is.InstanceOf<DateTime>());
        Assert.That(actual, Is.EqualTo(new DateTime(2012, 05, 10)));
    }

    [Test]
    public void Convert_StringToDateTime_YMDHMCorrectValue()
    {
        //Build the SUT
        var tc = new TypeConverter();

        //Call the method to test
        var actual = tc.Convert("2012-05-10 10:15", typeof(DateTime));

        //Assertion
        Assert.That(actual, Is.InstanceOf<DateTime>());
        Assert.That(actual, Is.EqualTo(new DateTime(2012, 05, 10, 10, 15, 0)));
    }
}
