using NBi.Core.Query;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Query
{
    [TestFixture]
    public class QueryEngineFactoryTest
    {
        [Test]
        public void GetExecutor_SqlNativeConnectionString_SqlNativeEngine()
        {
            var factory = new QueryEngineFactory();
            var cmd = new SqlCommand("select @@version;", new SqlConnection(ConnectionStringReader.GetSqlClient()));
            var executor = factory.GetExecutor(cmd);

            Assert.IsInstanceOf<QuerySqlEngine>(executor);
        }

        [Test]
        public void GetExecutor_OleDbConnectionString_OleDbEngine()
        {
            var factory = new QueryEngineFactory();
            var cmd = new OleDbCommand("select @@version;", new OleDbConnection(ConnectionStringReader.GetOleDbSql()));
            var executor = factory.GetExecutor(cmd);

            Assert.IsInstanceOf<QueryOleDbEngine>(executor);
        }

        [Test]
        public void GetExecutor_OdbcConnectionString_OdbcEngine()
        {
            var factory = new QueryEngineFactory();
            var cmd = new OdbcCommand("select @@version;", new OdbcConnection(ConnectionStringReader.GetOdbcSql()));
            var executor = factory.GetExecutor(cmd);

            Assert.IsInstanceOf<QueryOdbcEngine>(executor);
        }
    }
}
