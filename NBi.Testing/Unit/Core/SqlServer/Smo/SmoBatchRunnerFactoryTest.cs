using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Batch;
using System.Data.SqlClient;
using NUnit.Framework;
using Moq;
using NBi.Core.SqlServer.Smo;
using System.Data.OleDb;

namespace NBi.Testing.Unit.Core.SqlServer.Smo
{
    [TestFixture]
    public class SmoBatchRunnerFactoryTest
    {
        [Test]
        public void Get_NullConnection_Exception()
        {
            var mock = new Mock<IBatchRunCommand>();
            mock.SetupGet(x => x.FullPath).Returns("C:\foo.sql");
            var cmd = mock.Object;

            IDbConnection conn = null;
            var factory = new SmoBatchRunnerFactory();
            var ex = Assert.Throws<ArgumentNullException>(() => factory.Get(cmd, conn));
            Assert.That(ex.ParamName, Is.StringContaining("connection"));
        }

        [Test]
        public void Get_EmptyConnection_Exception()
        {
            var mock = new Mock<IBatchRunCommand>();
            mock.SetupGet(x => x.FullPath).Returns("C:\foo.sql");
            var cmd = mock.Object;

            IDbConnection conn = new SqlConnection();
            conn.ConnectionString = string.Empty;
            var factory = new SmoBatchRunnerFactory();
            var ex = Assert.Throws<ArgumentNullException>(() => factory.Get(cmd, conn));
            Assert.That(ex.Message, Is.StringContaining("No connection-string defined for the sql-run"));
        }

        [Test]
        public void Get_OleDbConnection_Exception()
        {
            var mock = new Mock<IBatchRunCommand>();
            mock.SetupGet(x => x.FullPath).Returns("C:\foo.sql");
            var cmd = mock.Object;

            IDbConnection conn = new OleDbConnection();
            conn.ConnectionString = ConnectionStringReader.GetOleDbSql();
            var factory = new SmoBatchRunnerFactory();
            var ex = Assert.Throws<ArgumentException>(() => factory.Get(cmd, conn));
            Assert.That(ex.Message, Is.StringContaining("SqlConnection"));
            Assert.That(ex.Message, Is.StringContaining("OleDbConnection"));
        }

        [Test]
        public void Get_SqlConnection_Instantiated()
        {
            var mock = new Mock<IBatchRunCommand>();
            mock.SetupGet(x => x.FullPath).Returns("C:\foo.sql");
            var cmd = mock.Object;

            IDbConnection conn = new SqlConnection();
            conn.ConnectionString = ConnectionStringReader.GetSqlClient();
            var factory = new SmoBatchRunnerFactory();
            var batchRun = factory.Get(cmd, conn);
            Assert.That(batchRun, Is.Not.Null);
        }
        
    }
}
