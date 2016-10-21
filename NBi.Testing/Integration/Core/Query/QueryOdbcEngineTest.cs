using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data.SqlClient;
using NBi.Core;
using System.Data.Odbc;

namespace NBi.Testing.Integration.Core.Query
{
    [TestFixture]
    public class QueryOdbcEngineTest
    {
        [Test]
        public void Execute_WaitFor3SecondsTimeoutSetTo1_Timeout()
        {
            var query = "WAITFOR DELAY '00:00:10';";
            var cmd = new OdbcCommand(query, new OdbcConnection(ConnectionStringReader.GetOdbcSql()));
            cmd.CommandTimeout = 1;

            var qe = new QueryOdbcEngine(cmd);
            Assert.Throws<CommandTimeoutException>(delegate { qe.Execute(); });   
        }

        [Test]
        public void Execute_WaitFor3SecondsTimeoutSetTo0_NoTimeOut()
        {

            var query = "WAITFOR DELAY '00:00:03';";
            var cmd = new OdbcCommand(query, new OdbcConnection(ConnectionStringReader.GetOdbcSql()));
            cmd.CommandTimeout = 0;

            var qe = new QueryOdbcEngine(cmd);
            Assert.DoesNotThrow(delegate { qe.Execute(); });

        }
        
    }
}
