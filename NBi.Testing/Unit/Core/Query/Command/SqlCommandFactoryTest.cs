using Moq;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Testing.Unit.Core.Query.Command
{
    [TestFixture]
    public class SqlCommandFactoryTest
    {
        [Test]
        public void Build_TimeoutSpecified_TimeoutSet()
        {
            var conn = new DbClient(DbProviderFactories.GetFactory("System.Data.SqlClient"), typeof(SqlConnection), "Data Source=server;Initial Catalog=database;Integrated Security=SSPI");
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == "Data Source=server;Initial Catalog=database;Integrated Security=SSPI"
                && x.Statement == "WAITFOR DELAY '00:00:15'"
                && x.Timeout == new TimeSpan(0, 0, 5)
                );

            var factory = new SqlCommandFactory();
            var cmd = factory.Instantiate(conn, query);
            Assert.IsInstanceOf<SqlCommand>(cmd.Implementation);
            Assert.That((cmd.Implementation as SqlCommand).CommandTimeout, Is.EqualTo(5));
        }

        [Test]
        public void Build_TimeoutSetToZero_TimeoutSet0Seconds()
        {
            var conn = new DbClient(DbProviderFactories.GetFactory("System.Data.SqlClient"), typeof(SqlConnection), "Data Source=server;Initial Catalog=database;Integrated Security=SSPI");
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == "Data Source=server;Initial Catalog=database;Integrated Security=SSPI"
                && x.Statement == "WAITFOR DELAY '00:00:15'"
                && x.Timeout == new TimeSpan(0, 0, 0)
                );

            var factory = new SqlCommandFactory();
            var cmd = factory.Instantiate(conn, query);
            Assert.IsInstanceOf<SqlCommand>(cmd.Implementation);
            Assert.That((cmd.Implementation as SqlCommand).CommandTimeout, Is.EqualTo(0));
        }

        [Test]
        public void Build_TimeoutSetTo30_TimeoutSet30Seconds()
        {
            var conn = new DbClient(DbProviderFactories.GetFactory("System.Data.SqlClient"), typeof(SqlConnection), "Data Source=server;Initial Catalog=database;Integrated Security=SSPI");
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == "Data Source=server;Initial Catalog=database;Integrated Security=SSPI"
                && x.Statement == "WAITFOR DELAY '00:00:15'"
                && x.Timeout == new TimeSpan(0, 0, 30)
                );

            var factory = new SqlCommandFactory();
            var cmd = factory.Instantiate(conn, query);
            Assert.IsInstanceOf<SqlCommand>(cmd.Implementation);
            Assert.That((cmd.Implementation as SqlCommand).CommandTimeout, Is.EqualTo(30));
        }

        [Test]
        public void Build_CommandTypeSetToText_CommandTypeSetText()
        {
            var conn = new DbClient(DbProviderFactories.GetFactory("System.Data.SqlClient"), typeof(SqlConnection), "Data Source=server;Initial Catalog=database;Integrated Security=SSPI");
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == "Data Source=server;Initial Catalog=database;Integrated Security=SSPI"
                && x.CommandType == System.Data.CommandType.Text
                );

            var factory = new SqlCommandFactory();
            var cmd = factory.Instantiate(conn, query);
            Assert.IsInstanceOf<SqlCommand>(cmd.Implementation);
            Assert.That((cmd.Implementation as SqlCommand).CommandType, Is.EqualTo(System.Data.CommandType.Text));
        }

        [Test]
        public void Build_CommandTypeSetToStoredProcedure_CommandTypeSetStoredProcedure()
        {
            var conn = new DbClient(DbProviderFactories.GetFactory("System.Data.SqlClient"), typeof(SqlConnection), "Data Source=server;Initial Catalog=database;Integrated Security=SSPI");
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == "Data Source=server;Initial Catalog=database;Integrated Security=SSPI"
                && x.CommandType == System.Data.CommandType.StoredProcedure
                );

            var factory = new SqlCommandFactory();
            var cmd = factory.Instantiate(conn, query);
            Assert.IsInstanceOf<SqlCommand>(cmd.Implementation);
            Assert.That((cmd.Implementation as SqlCommand).CommandType, Is.EqualTo(System.Data.CommandType.StoredProcedure));
        }
    }
}
