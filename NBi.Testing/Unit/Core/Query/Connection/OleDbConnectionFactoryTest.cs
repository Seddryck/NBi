#region Using directives
using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Connection;
using NUnit.Framework;
using System.Collections.Generic;
using NBi.Core.PowerBiDesktop;

#endregion

namespace NBi.Testing.Unit.Core.Query.Connection
{
    [TestFixture]
    public class OleDbConnectionFactoryTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
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
        public void Get_OleDb_OledbConnection()
        {
            var connStr = "Provider=OledB;Data Source=ds;Initial Catalog=ic";
            var actual = new OleDbConnectionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlOleDb_OledbConnection()
        {
            var connStr = "Provider=sqlOledB;Data Source=ds;Initial Catalog=ic";
            var actual = new OleDbConnectionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlNCli_OleDbConnection()
        {
            var connStr = "Provider=SQLNCLI;Data Source=ds;Initial Catalog=ic";
            var actual = new OleDbConnectionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlNCli10Dot1_OleDbConnection()
        {
            var connStr = "Provider=SQLNCLI10.1;Data Source=ds;Initial Catalog=ic;Integrated Security=SSPI;";
            var actual = new OleDbConnectionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlNCli11Dot1_OleDbConnection()
        {
            var connStr = "Provider=SQLNCLI11.1;Data Source=.;Initial Catalog=AdventureWorks2014;Integrated Security=SSPI";
            var actual = new OleDbConnectionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_ConnectionStringOleDbProvider_OleDbConnection()
        {
            var connStr = "Provider=OleDb.1;Data Source=ds;Initial Catalog=ic;Integrated Security=SSPI;";
            var actual = new OleDbConnectionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_OleDbExcel_OleDbConnection()
        {
            //Call the method to test
            var connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\myFolder\\myExcel2007file.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES\";";
            var providers = new Dictionary<string, string>() { { "Microsoft.ACE.OLEDB.12.0", "System.Data.OleDb" } };
            var factory = new OleDbConnectionFactory(providers);
            var actual = factory.Instantiate(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<DbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }
    }
}
