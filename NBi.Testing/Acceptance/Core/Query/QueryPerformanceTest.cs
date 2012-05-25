using System.Data.SqlClient;
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

            var qp = new QueryEngineFactory().GetPerformance(cmd);
            var res = qp.CheckPerformance();

            Assert.That(res.TimeElapsed.TotalMilliseconds, Is.GreaterThanOrEqualTo(0).And.LessThan(5000));
        }
    }
}
