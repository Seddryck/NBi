using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using DubUrl.Extensions;
using System.Data;

namespace NBi.Core.Testing.Query.Client;

[TestFixture]
public class DubUrlClientFactoryTest
{
    [Test]
    [TestCase("mssql://server/db")]
    [TestCase("odbc+mssql://server/db")]
    [TestCase("oledb+xlsx:///customer.xlsx")]
    [TestCase("pbix:///customer")]
    [TestCase("pbipremium://api.powerbi.com/v1.0/myOrganization/myWorkspace")]
    public void Instantiate_ConnectionUrl_DubUrlClient(string connectionUrl)
    {
        var client = new DubUrlClientFactory().Instantiate(connectionUrl);

        Assert.That(client, Is.InstanceOf<DubUrlClient>());
        Assert.That(client.ConnectionString, Is.EqualTo(connectionUrl));
    }

    [Test]
    [TestCase("mssql://server/db", "Data Source=server;Initial Catalog=db;Integrated Security=True")]
    [TestCase("odbc+mssql://server/db", "Driver={ODBC Driver 17 for SQL Server};Server=server;Database=db")]
    [TestCase("oledb+xlsx:///customer.xlsx", "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=customer.xlsx;Extended Properties=\"Excel 12.0 Xml;\"")]
    //[TestCase("pbix:///customer", "...")]
    [TestCase("pbipremium://api.powerbi.com/v1.0/myOrganization/myWorkspace", "Data Source=powerbi://api.powerbi.com/v1.0/myOrganization/myWorkspace")]
    public void Instantiate_ConnectionUrl_Connection(string connectionUrl, string expected)
    {
        var conn = new DubUrlClientFactory().Instantiate(connectionUrl).CreateNew();
        Assert.That(conn, Is.AssignableTo<IDbConnection>());
        Assert.That(((IDbConnection)conn).ConnectionString, Is.EqualTo(expected));
    }
}
