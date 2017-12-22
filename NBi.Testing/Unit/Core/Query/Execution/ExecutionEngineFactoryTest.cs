using Moq;
using NBi.Core.Query;
using NBi.Core.Query.Execution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Query.Execution
{
    public class ExecutionEngineFactoryTest
    {
        [Test]
        public void Instantiate_SqlClient_SqlExecutionEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select 1"
                );

            var factory = new ExecutionEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<SqlExecutionEngine>(engine);
        }

        [Test]
        public void Instantiate_Adomd_AdomdExecutionEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetAdomd()
                && x.Statement == "select 1 on 0"
                );

            var factory = new ExecutionEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<AdomdExecutionEngine>(engine);
        }

        [Test]
        public void Instantiate_Odbc_OdbcExecutionEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetOdbcSql()
                && x.Statement == "select 1"
                );

            var factory = new ExecutionEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<OdbcExecutionEngine>(engine);
        }

        [Test]
        public void Instantiate_OleDb_OleDbExecutionEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetOleDbSql()
                && x.Statement == "select 1"
                );

            var factory = new ExecutionEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<OleDbExecutionEngine>(engine);
        }
    }
}
