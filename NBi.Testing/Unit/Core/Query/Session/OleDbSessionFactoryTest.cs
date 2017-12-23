#region Using directives
using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Session;
using NUnit.Framework;
using System.Collections.Generic;
using NBi.Core.PowerBiDesktop;
using Moq;
using NBi.Core.Configuration;

#endregion

namespace NBi.Testing.Unit.Core.Query.Session
{
    [TestFixture]
    public class OleDbSessionFactoryTest
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
            var actual = new OleDbSessionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbSession>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlOleDb_OledbConnection()
        {
            var connStr = "Provider=sqlOledB;Data Source=ds;Initial Catalog=ic";
            var actual = new OleDbSessionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbSession>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlNCli_OleDbConnection()
        {
            var connStr = "Provider=SQLNCLI;Data Source=ds;Initial Catalog=ic";
            var actual = new OleDbSessionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbSession>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlNCli10Dot1_OleDbConnection()
        {
            var connStr = "Provider=SQLNCLI10.1;Data Source=ds;Initial Catalog=ic;Integrated Security=SSPI;";
            var actual = new OleDbSessionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbSession>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlNCli11Dot1_OleDbConnection()
        {
            var connStr = "Provider=SQLNCLI11.1;Data Source=.;Initial Catalog=AdventureWorks2014;Integrated Security=SSPI";
            var actual = new OleDbSessionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbSession>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_ConnectionStringOleDbProvider_OleDbConnection()
        {
            var connStr = "Provider=OleDb.1;Data Source=ds;Initial Catalog=ic;Integrated Security=SSPI;";
            var actual = new OleDbSessionFactory().Instantiate(connStr);

            Assert.That(actual, Is.InstanceOf<DbSession>());
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
            var providersConfig = Mock.Of<IProvidersConfiguration>(x => x.Providers == providers);

            var factory = new OleDbSessionFactory(providersConfig);
            var actual = factory.Instantiate(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<DbSession>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
            var conn = actual.CreateNew();

            Assert.That(conn, Is.InstanceOf<OleDbConnection>());
            Assert.That((conn as OleDbConnection).ConnectionString, Is.EqualTo(connStr));
        }
    }
}
