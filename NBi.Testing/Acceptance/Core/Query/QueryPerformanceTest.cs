using System.Data.SqlClient;
using NBi.Core;
using NBi.Core.Query;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.Core.Query
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
        public void CheckPerformance_OneQuery_ReturnElapsedTime()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            IQueryPerformance qp = new QuerySqlEngine(ConnectionStringReader.GetSqlClient());
            var res = qp.CheckPerformance(cmd, true);

            Assert.That(res.TimeElapsed.TotalMilliseconds, Is.GreaterThan(0).And.LessThan(5000));
        }
    }
}
