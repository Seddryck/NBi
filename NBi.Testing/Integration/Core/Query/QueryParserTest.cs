#region Using directives
using System.Data.SqlClient;
using System.Xml.Schema;
using NBi.Core;
using NBi.Core.Query;
using NUnit.Framework;
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

        [Test]
        public void Parse_CorrectTableName_Success()
        {
            var sql = "SELECT * FROM [HumanResources].[Department];";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.True);

        }

        [Test]
        public void Parse_WrongTableName_Failed()
        {
            var sql = "SELECT * FROM WrongTableName;";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Invalid object name 'WrongTableName'."));
        }

        [Test]
        public void Parse_CorrectFields_Success()
        {
            var sql = "select [DepartmentID], Name from [HumanResources].[Department];";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.True);

        }

        [Test]
        public void Parse_WrongField_Failed()
        {
            var sql = "select [DepartmentID], Name, WrongField from [HumanResources].[Department];";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Invalid column name 'WrongField'."));
        }

        [Test]
        public void Parse_WrongFields_Failed()
        {
            var sql = "select [DepartmentID], Name, WrongField1, WrongField2 from [HumanResources].[Department];";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Invalid column name 'WrongField1'."));
            Assert.That(res.Errors[1], Is.EqualTo("Invalid column name 'WrongField2'."));
        }

        [Test]
        public void Parse_WrongSyntax_Failed()
        {
            var sql = "SELECTION [DepartmentID], Name, WrongField1, WrongField2 from [HumanResources].[Department];";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var qp = new QueryEngineFactory().GetParser(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Incorrect syntax near 'SELECTION'."));
        }


        [Test]
        public void Parse_DontExecuteEffectivelyQuery()
        {
            var sqlCount = @"SELECT COUNT(*) from [HumanResources].[Department]";
            var sql = @"DELETE from [HumanResources].[Department]";

            var countBefore = ExecuteCount(sqlCount);
            if (countBefore == 0) //If nothing was present we cannot assert
                Assert.Inconclusive();

            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));
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
