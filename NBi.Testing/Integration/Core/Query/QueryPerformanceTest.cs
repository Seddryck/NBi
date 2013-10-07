using System.Data.SqlClient;
using NBi.Core.Query;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Query
{
    [TestFixture]
    [Category("Sql")]
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
            var sql = "WAITFOR DELAY '00:00:00';";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var qp = new QueryEngineFactory().GetPerformance(cmd);
            var res = qp.CheckPerformance();

            Assert.That(res.TimeElapsed.TotalMilliseconds, Is.GreaterThanOrEqualTo(0).And.LessThan(5000));
            Assert.That(res.IsTimeOut, Is.False);
        }

        [Test]
        public void CheckPerformance_OneQueryHavingTimeout_ReturnTimeoutInfo()
        {
            var sql = "WAITFOR DELAY '00:00:03';";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var qp = new QueryEngineFactory().GetPerformance(cmd);
            var res = qp.CheckPerformance(1000);

            Assert.That(res.TimeOut.TotalMilliseconds, Is.EqualTo(1000));
            Assert.That(res.IsTimeOut, Is.True);
        }
    }
}
