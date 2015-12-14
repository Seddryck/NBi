#region Using directives
using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core;
using NUnit.Framework;
using System.Collections.Generic;
using NBi.Core.PowerBiDesktop;

#endregion

namespace NBi.Testing.Unit.Core
{
    [TestFixture]
    public class ConnectionFactoryTest
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
        public void Get_MsOlap_OleDbConnection()
        {
            //Call the method to test
            var connStr = "Provider=MSOLAP;Data Source=ds;Initial Catalog=ic";
            var actual = new ConnectionFactory().Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<AdomdConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_MsOlapDot4_OledbConnection()
        {
            //Call the method to test
            var connStr = "Provider=msOlaP.4;Data Source=ds;Initial Catalog=ic";
            var actual = new ConnectionFactory().Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<AdomdConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_OleDb_OledbConnection()
        {
            //Call the method to test
            var connStr = "Provider=OledB;Data Source=ds;Initial Catalog=ic";
            var actual = new ConnectionFactory().Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlNCli_OleDbConnection()
        {
            //Call the method to test
            var connStr = "Provider=SQLNCLI;Data Source=ds;Initial Catalog=ic";
            var actual = new ConnectionFactory().Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlNCli10Dot1_OleDbConnection()
        {
            //Call the method to test
            var connStr = "Provider=SQLNCLI10.1;Data Source=ds;Initial Catalog=ic;Integrated Security=SSPI;";
            var actual = new ConnectionFactory().Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlNCli11Dot1_OleDbConnection()
        {
            //Call the method to test
            var connStr = "Provider=SQLNCLI11.1;Data Source=.;Initial Catalog=AdventureWorks2014;Integrated Security=SSPI";
            var actual = new ConnectionFactory().Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_Odbc_OdbcConnection()
        {
            //Call the method to test
            var connStr = "Driver={SQL Server Native Client 10.0};Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
            var actual = new ConnectionFactory().Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OdbcConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }
        
        [Test]
        public void Get_NoProviderDefined_SqlConnection()
        {
            var connStr = "Data Source=ds;Initial Catalog=ic";

            //Call the method to test
            var actual = new ConnectionFactory().Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<SqlConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_ConnectionStringOleDbProvider_OleDbConnection()
        {
            //Call the method to test
            var connStr = "Provider=OleDb.1;Data Source=ds;Initial Catalog=ic;Integrated Security=SSPI;";
            var actual = new ConnectionFactory().Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_OleDbExcel_OleDbConnection()
        {
            //Call the method to test
            var connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\\myFolder\\myExcel2007file.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES\";";
            var providers = new Dictionary<string, string>();
            providers.Add("Microsoft.ACE.OLEDB.12.0", "System.Data.OleDb");
            var factory = new ConnectionFactory(providers);
            var actual = factory.Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        #region Power BI Desktop
        
        private class ConnectionFactoryPowerBiDesktopFake : ConnectionFactory
        {
            protected override PowerBiDesktopConnectionStringBuilder GetPowerBiDesktopConnectionStringBuilder()
            {
                return new PowerBiDesktopConnectionStringBuilderFake();
            }
        }

        private class PowerBiDesktopConnectionStringBuilderFake : PowerBiDesktopConnectionStringBuilder
        {
            public static string ConnectionString = "Data Source=localhost:2325;";
            protected override string BuildLocalConnectionString(string name)
            {
                return ConnectionString;
            }
        }

        #endregion

        [Test]
        public void Get_PowerBiDesktop_AdommdConnection()
        {
            //Call the method to test
            var connStr = "PBIX=My Power BI Desktop;";
            var factory = new ConnectionFactoryPowerBiDesktopFake();
            var actual = factory.Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<AdomdConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(PowerBiDesktopConnectionStringBuilderFake.ConnectionString));
        }
    }
}
