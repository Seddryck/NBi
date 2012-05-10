using System.Data.SqlClient;
using NBi.Core;
using NBi.Core.Database;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.Core.Database
{
    [TestFixture]
    public class QueryPerformanceTest
    {

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void Validate_LessThan5000MilliSeconds_Success()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var qp = new QueryPerformance(5000, true);
            var res = qp.Validate(cmd);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Success));
        }

        [Test]
        public void Validate_LessThan0MilliSeconds_Failed()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var qp = new QueryPerformance(-1, true);
            var res = qp.Validate(cmd);

            Assert.That(res.Value, Is.EqualTo(Result.ValueType.Failed));
            Assert.That(res.Failures[0], Is.StringStarting("Maximum time specified was"));
        }
    }
}
