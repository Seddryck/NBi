using System;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data.OleDb;
using NBi.Core.Query.Execution;
using NBi.Core;
using NBi.Core.Query.Performance;

namespace NBi.Testing.Integration.Core.Query.Execution
{
    [TestFixture]
    public class OleDbPerformanceEngineTest
    {
        [Test]
        public void CheckPerformance_OneQuery_ReturnElapsedTime()
        {
            var sql = "WAITFOR DELAY '00:00:00';";
            var cmd = new OleDbCommand(sql, new OleDbConnection(ConnectionStringReader.GetOleDbSql()));

            var qp = new OleDbPerformanceEngine(cmd);
            var res = qp.Execute(new TimeSpan(0, 1, 0));

            Assert.That(res.TimeElapsed.TotalMilliseconds, Is.GreaterThanOrEqualTo(0).And.LessThan(5000));
            Assert.That(res.IsTimeOut, Is.False);
        }

        [Test]
        public void Execute_OneQueryHavingTimeout_ReturnTimeoutInfo()
        {
            var query = "WAITFOR DELAY '00:00:03';";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetOleDbSql()));

            var qp = new OleDbPerformanceEngine(cmd);
            var res = qp.Execute(new TimeSpan(0, 0, 1));

            Assert.That(res.TimeOut.TotalMilliseconds, Is.EqualTo(1000));
            Assert.That(res.IsTimeOut, Is.True);
        }

        [Test]
        [Category("LocalSQL")]
        public void CleanCache_Any_DoesNotThrow()
        {
            var query = "WAITFOR DELAY '00:00:03';";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetLocalOleDbSql()));

            var qp = new OleDbPerformanceEngine(cmd);
            Assert.DoesNotThrow(() => qp.CleanCache());
        }
    }
}
