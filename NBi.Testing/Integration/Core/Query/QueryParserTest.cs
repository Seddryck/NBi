#region Using directives
using System.Data.SqlClient;
using System.Xml.Schema;
using NBi.Core;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data.Odbc;
using System.Data;
using System;
using System.Data.OleDb;
#endregion

namespace NBi.Testing.Integration.Core.Query
{
    [TestFixture]
    [Category("Sql")]
    public class QueryParserTest
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

        public enum EngineType
        {
            SqlNative
            , OleDb
            , Odbc
        }

        private IDbCommand BuildCommand(string sql, EngineType engineType)
        {
            switch (engineType)
            {
                case EngineType.SqlNative:
                    return new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));
                case EngineType.OleDb:
                    return new OleDbCommand(sql, new OleDbConnection(ConnectionStringReader.GetOleDbSql()));
                case EngineType.Odbc:
                    return new OdbcCommand(sql, new OdbcConnection(ConnectionStringReader.GetOdbcSql()));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Test]
        [TestCase(EngineType.SqlNative)]
        [TestCase(EngineType.OleDb)]
        [TestCase(EngineType.Odbc)]
        public void Parse_CorrectTableName_Success(EngineType engineType)
        {
            var sql = "SELECT * FROM [HumanResources].[Department];";
            var cmd = BuildCommand(sql, engineType);
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.True);

        }

        [Test]
        [TestCase(EngineType.SqlNative)]
        [TestCase(EngineType.OleDb)]
        [TestCase(EngineType.Odbc)]
        public void Parse_WrongTableName_Failed(EngineType engineType)
        {
            var sql = "SELECT * FROM WrongTableName;";
            var cmd = BuildCommand(sql, engineType);
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Invalid object name 'WrongTableName'."));
        }

        [Test]
        [TestCase(EngineType.SqlNative)]
        [TestCase(EngineType.OleDb)]
        [TestCase(EngineType.Odbc)]
        public void Parse_CorrectFields_Success(EngineType engineType)
        {
            var sql = "select [DepartmentID], Name from [HumanResources].[Department];";
            var cmd = BuildCommand(sql, engineType);
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.True);

        }

        [Test]
        [TestCase(EngineType.SqlNative)]
        [TestCase(EngineType.OleDb)]
        [TestCase(EngineType.Odbc)]
        public void Parse_WrongField_Failed(EngineType engineType)
        {
            var sql = "select [DepartmentID], Name, WrongField from [HumanResources].[Department];";
            var cmd = BuildCommand(sql, engineType);
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Invalid column name 'WrongField'."));
        }

        [Test]
        [TestCase(EngineType.SqlNative)]
        [TestCase(EngineType.OleDb)]
        [TestCase(EngineType.Odbc)]
        public void Parse_WrongFields_Failed(EngineType engineType)
        {
            var sql = "select [DepartmentID], Name, WrongField1, WrongField2 from [HumanResources].[Department];";
            var cmd = BuildCommand(sql, engineType);
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors, Has.Member("Invalid column name 'WrongField1'."));
            Assert.That(res.Errors, Has.Member("Invalid column name 'WrongField2'."));
        }

        [Test]
        [TestCase(EngineType.SqlNative)]
        [TestCase(EngineType.OleDb)]
        [TestCase(EngineType.Odbc)]
        public void Parse_WrongSyntax_Failed(EngineType engineType)
        {
            var sql = "SELECTION [DepartmentID], Name, WrongField1, WrongField2 from [HumanResources].[Department];";
            var cmd = BuildCommand(sql, engineType);
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Incorrect syntax near 'SELECTION'."));
        }


        [Test]
        [TestCase(EngineType.SqlNative)]
        [TestCase(EngineType.OleDb)]
        [TestCase(EngineType.Odbc)]
        public void Parse_DontExecuteEffectivelyQuery(EngineType engineType)
        {
            var sqlCount = @"SELECT COUNT(*) from [HumanResources].[Department]";
            var sql = @"DELETE from [HumanResources].[Department]";

            var countBefore = ExecuteCount(sqlCount);
            if (countBefore == 0) //If nothing was present we cannot assert
                Assert.Inconclusive();

            var cmd = BuildCommand(sql, engineType);
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            if (!res.IsSuccesful)//If syntax is incorrect we cannot assert
                Assert.Inconclusive();

            var countAfter = ExecuteCount(sqlCount);

            Assert.That(countAfter, Is.EqualTo(countBefore));
        }

        private int ExecuteCount(string sqlCount)
        {
            int count;

            using (SqlConnection conn = new SqlConnection(ConnectionStringReader.GetSqlClient()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sqlCount, conn))
                {
                    count = (int)cmd.ExecuteScalar();
                }

                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

            return count;
        }

        

    }
}
