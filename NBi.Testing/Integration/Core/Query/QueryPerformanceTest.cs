using System.Data.SqlClient;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data.OleDb;
using System.Data.Odbc;
using System;
using System.Data;

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
        public void CheckPerformance_OneQuery_ReturnElapsedTime(EngineType engineType)
        {
            var sql = "WAITFOR DELAY '00:00:00';";
            var cmd = BuildCommand(sql, engineType);

            var qp = new QueryEngineFactory().GetPerformance(cmd);
            var res = qp.CheckPerformance();

            Assert.That(res.TimeElapsed.TotalMilliseconds, Is.GreaterThanOrEqualTo(0).And.LessThan(5000));
            Assert.That(res.IsTimeOut, Is.False);
        }

        [Test]
        [TestCase(EngineType.SqlNative)]
        [TestCase(EngineType.OleDb)]
        [TestCase(EngineType.Odbc)]
        public void CheckPerformance_OneQueryHavingTimeout_ReturnTimeoutInfo(EngineType engineType)
        {
            var sql = "WAITFOR DELAY '00:00:03';";
            var cmd = BuildCommand(sql, engineType);

            var qp = new QueryEngineFactory().GetPerformance(cmd);
            var res = qp.CheckPerformance(1000);

            Assert.That(res.TimeOut.TotalMilliseconds, Is.EqualTo(1000));
            Assert.That(res.IsTimeOut, Is.True);
        }
    }
}
