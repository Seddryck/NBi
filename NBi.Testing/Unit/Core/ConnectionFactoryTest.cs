#region Using directives
using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core;
using NUnit.Framework;

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
        public void Get_OleDbMSOLAP_OledbConnection()
        {
            //Call the method to test
            var connStr = "Provider=MSOLAP;Data Source=ds;Initial Catalog=ic";
            var actual = ConnectionFactory.Get("OleDb", connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_oLEdBWithIncorrectCase_OledbConnection()
        {
            //Call the method to test
            var connStr = "Provider=MSOLAP;Data Source=ds;Initial Catalog=ic";
            var actual = ConnectionFactory.Get("OleDb", connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SystemDataOleDb_OledbConnection()
        {
            //Call the method to test
            var connStr = "Provider=MSOLAP;Data Source=ds;Initial Catalog=ic";
            var actual = ConnectionFactory.Get("System.Data.OleDb", connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_OleDbSQLNCLI_OledbConnection()
        {
            //Call the method to test
            var connStr = "Provider=SQLNCLI;Data Source=ds;Initial Catalog=ic";
            var actual = ConnectionFactory.Get("OleDb", connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OleDbConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_Odbc_OdbcConnection()
        {
            //Call the method to test
            var connStr = "Driver={SQL Server Native Client 10.0};Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
            var actual = ConnectionFactory.Get("Odbc", connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<OdbcConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }
        
        [Test]
        public void Get_SqlClient_SqlConnection()
        {
            var connStr = "Data Source=ds;Initial Catalog=ic";

            //Call the method to test
            var actual = ConnectionFactory.Get("SqlClient", connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<SqlConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        
        [Test]
        public void Get_Adomd_SqlConnection()
        {
            var connStr = "Data Source=ds;Initial Catalog=ic";

            //Call the method to test
            var actual = ConnectionFactory.Get("Adomd", connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<AdomdConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_SqlIsAmbiguousWithSqlClientAndSqlServerCe_ThrowsArgumentException()
        {
            var connStr = "Data Source=ds;Initial Catalog=ic";

            //Call the method to test and Assert
            Assert.Throws<ArgumentException>(delegate { ConnectionFactory.Get("Sql", connStr); });
        }

        [Test]
        public void Get_NotExistingProvider_ThrowsArgumentException()
        {
            var connStr = "Data Source=ds;Initial Catalog=ic";

            //Call the method to test and Assert
            Assert.Throws<ArgumentException>(delegate { ConnectionFactory.Get("NotExistingProvider", connStr); });
        }

        [Test]
        public void Get_ConnectionStringMsOlapProvider_AdomdConnection()
        {
            //Call the method to test
            var connStr = "Provider=MSOLAP;Data Source=ds;Initial Catalog=ic";
            var actual = ConnectionFactory.Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<AdomdConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

        [Test]
        public void Get_ConnectionStringEmptypProvider_SqlConnection()
        {
            //Call the method to test
            var connStr = "Data Source=ds;Initial Catalog=ic";
            var actual = ConnectionFactory.Get(connStr);

            //Assertion
            Assert.That(actual, Is.InstanceOf<SqlConnection>());
            Assert.That(actual.ConnectionString, Is.EqualTo(connStr));
        }

    }
}
