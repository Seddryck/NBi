using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data.SqlClient;
using NBi.Core;
using System.Data.OleDb;

namespace NBi.Testing.Integration.Core.Query
{
    [TestFixture]
    public class QueryOleDbEngineTest
    {
        [Test]
        public void Execute_WaitFor3SecondsTimeoutSetTo1_Timeout()
        {
            var query = "WAITFOR DELAY '00:00:03';";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetOleDb()));
            cmd.CommandTimeout = 1;

            var qe = new QueryOleDbEngine(cmd);
            Assert.Throws<CommandTimeoutException>(delegate { qe.Execute(); });   
        }

        [Test]
        public void Execute_WaitFor3SecondsTimeoutSetTo0_NoTimeOut()
        {

            var query = "WAITFOR DELAY '00:00:03';";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetOleDb()));
            cmd.CommandTimeout = 0;

            var qe = new QueryOleDbEngine(cmd);
            Assert.DoesNotThrow(delegate { qe.Execute(); });

        }
    }
}
