using System.Data.SqlClient;
using NBi.Core;
using NBi.Core.Database;
using NUnit.Framework;

namespace NBi.Testing.Database
{
    [TestFixture]
    public class QueryParserTest 
    {

        protected string _connectionString;

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
            _connectionString = "Data Source=.;Initial Catalog=NBi.Testing;Integrated Security=True";
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void ValidateFormat_CorrectTableName_Success()
        {
            var sql = "SELECT * FROM Product;";

            var qp = new QueryParser(_connectionString, sql);
            var res = qp.ValidateFormat();

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));
            
        }

        [Test]
        public void ValidateFormat_WrongTableName_Failed()
        {
            var sql = "SELECT * FROM WrongTableName;";

            var qp = new QueryParser(_connectionString, sql);
            var res = qp.ValidateFormat();

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.EqualTo("Invalid object name 'WrongTableName'."));
        }

        [Test]
        public void ValidateFormat_CorrectFields_Success()
        {
            var sql = "SELECT ProductSKU, [Description] FROM Product;";

            var qp = new QueryParser(_connectionString, sql);
            var res = qp.ValidateFormat();

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));

        }

        [Test]
        public void ValidateFormat_WrongField_Failed()
        {
            var sql = "SELECT ProductSKU, [Description], WrongField FROM Product;";

            var qp = new QueryParser(_connectionString, sql);
            var res = qp.ValidateFormat();

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.EqualTo("Invalid column name 'WrongField'."));
        }

        [Test]
        public void ValidateFormat_WrongFields_Failed()
        {
            var sql = "SELECT ProductSKU, [Description], WrongField1, WrongField2 FROM Product;";

            var qp = new QueryParser(_connectionString, sql);
            var res = qp.ValidateFormat();

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.EqualTo("Invalid column name 'WrongField1'."));
            Assert.That(res.Failures[1], Is.EqualTo("Invalid column name 'WrongField2'."));
        }

        [Test]
        public void ValidateFormat_WrongSyntax_Failed()
        {
            var sql = "SELECTION ProductSKU, [Description], WrongField1, WrongField2 FROM Product;";

            var qp = new QueryParser(_connectionString, sql);
            var res = qp.ValidateFormat();

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.EqualTo("Incorrect syntax near 'SELECTION'."));
        }


        [Test]
        public void ValidateFormat_DontExecuteEffectivelyQuery()
        {
            var sqlCount = @"SELECT COUNT(*) FROM Product";
            var sql = @"DELETE FROM Product";

            var countBefore = ExecuteCount(sqlCount);
            if (countBefore == 0) //If nothing was present we cannot assert
                Assert.Inconclusive();

            var qp = new QueryParser(_connectionString, sql);
            var res = qp.ValidateFormat();

            if (res.Value != Result.ValueType.Success) //If syntax is incorrect we cannot assert
                Assert.Inconclusive();

            var countAfter=ExecuteCount(sqlCount);

            Assert.That(countAfter, Is.EqualTo(countBefore));
        }
  
        private int ExecuteCount(string sqlCount)
        {
            int count;

            using (SqlConnection conn = new SqlConnection(_connectionString))
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
