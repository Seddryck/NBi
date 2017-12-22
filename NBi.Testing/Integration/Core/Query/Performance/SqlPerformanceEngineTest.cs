using System;
using NUnit.Framework;
using System.Data.SqlClient;
using NBi.Core.Query.Performance;

namespace NBi.Testing.Integration.Core.Query.Performance
{
    [TestFixture]
    public class SqlPerformanceEngineTest
    {
        [Test]
        public void CheckPerformance_OneQuery_ReturnElapsedTime()
        {
            var sql = "WAITFOR DELAY '00:00:00';";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var qp = new SqlPerformanceEngine(cmd.Connection, cmd);
            var res = qp.Execute(new TimeSpan(0, 1, 0));

            Assert.That(res.TimeElapsed.TotalMilliseconds, Is.GreaterThanOrEqualTo(0).And.LessThan(5000));
            Assert.That(res.IsTimeOut, Is.False);
        }

        [Test]
        public void Execute_OneQueryHavingTimeout_ReturnTimeoutInfo()
        {
            var query = "WAITFOR DELAY '00:00:03';";
            var cmd = new SqlCommand(query, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var qp = new SqlPerformanceEngine(cmd.Connection, cmd);
            var res = qp.Execute(new TimeSpan(0,0,1));

            Assert.That(res.IsTimeOut, Is.True);
            Assert.That(res.TimeOut.TotalMilliseconds, Is.EqualTo(1000));
        }

        [Test]
        [Category("LocalSQL")]
        [Ignore("Privilege is too high")]
        public void CleanCache_Any_DoesNotThrow()
        {
            var query = "select 1;";
            var cmd = new SqlCommand(query, new SqlConnection(ConnectionStringReader.GetLocalSqlClient()));

            var qp = new SqlPerformanceEngine(cmd.Connection, cmd);
            Assert.DoesNotThrow(() => qp.CleanCache());
        }
    }
}
