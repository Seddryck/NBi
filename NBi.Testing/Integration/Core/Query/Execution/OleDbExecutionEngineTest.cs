using System;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data.OleDb;
using NBi.Core.Query.Execution;
using NBi.Core;

namespace NBi.Testing.Integration.Core.Query.Execution
{
    [TestFixture]
    public class OleDbExecutionEngineTest
    {
        [Test]
        public void Execute_WaitFor3SecondsTimeoutSetTo1_Timeout()
        {

            var query = "WAITFOR DELAY '00:00:03';";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetOleDbSql()))
            {
                CommandTimeout = 1
            };

            var qe = new OleDbExecutionEngine(cmd);
            Assert.Throws<CommandTimeoutException>(delegate { qe.Execute(); });
        }

        [Test]
        public void Execute_WaitFor3SecondsTimeoutSetTo0_NoTimeOut()
        {

            var query = "WAITFOR DELAY '00:00:03';";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetOleDbSql()))
            {
                CommandTimeout = 0
            };

            var qe = new OleDbExecutionEngine(cmd);
            Assert.DoesNotThrow(delegate { qe.Execute(); });

        }

        [Test]
        public void Execute_ValidQuery_DataSetFilled()
        {
            var query = "select * from [Sales].[Currency];";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetOleDbSql())) { CommandTimeout = 0 };

            var qe = new OleDbExecutionEngine(cmd);
            var ds = qe.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Columns, Has.Count.EqualTo(3));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(105));
        }

        [Test]
        public void ExecuteScalar_ValidQuery_DataSetFilled()
        {
            var query = "select top(1) CurrencyCode from [Sales].[Currency] where Name like '%Canad%'";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetOleDbSql())) { CommandTimeout = 0 };

            var qe = new OleDbExecutionEngine(cmd);
            var value = qe.ExecuteScalar();
            Assert.That(value, Is.EqualTo("CAD"));
        }

        [Test]
        public void ExecuteList_ValidQuery_DataSetFilled()
        {
            var query = "select top(10) CurrencyCode from [Sales].[Currency] where CurrencyCode like '%D' order by 1 asc";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetOleDbSql())) { CommandTimeout = 0 };

            var qe = new OleDbExecutionEngine(cmd);
            var values = qe.ExecuteList<string>();
            Assert.That(values, Has.Count.EqualTo(10));
            Assert.That(values, Has.Member("CAD"));
        }
    }
}
