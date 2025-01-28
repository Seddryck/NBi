#region Using directives
using System;
using System.Data.Odbc;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System.Collections.Generic;
using NBi.Core.PowerBiDesktop;

#endregion

namespace NBi.Core.Testing.Query.Client;

[TestFixture]
public class SqlClientFactoryTest
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
    public void Get_NoProviderDefined_SqlConnection()
    {
        var connStr = "Data Source=ds;Initial Catalog=ic";
        var actual = new NBi.Core.Query.Client.SqlClientFactory().Instantiate(connStr);

        Assert.That(actual, Is.InstanceOf<DbClient>());
        Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        var conn = actual.CreateNew();

        Assert.That(conn, Is.InstanceOf<SqlConnection>());
        Assert.That(((SqlConnection)conn).ConnectionString, Is.EqualTo(connStr));
    }
}
