using Moq;
using NBi.Core.Query;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Query
{
    [TestFixture]
    public class CommandBuilderTest
    {
        [Test]
        public void Build_TimeoutSpecified_TimeoutSet()
        {
            var conn = new SqlConnection();
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == "Data Source=server;Initial Catalog=database;Integrated Security=SSPI"
                && x.Statement == "WAITFOR DELAY '00:00:15'"
                && x.Timeout == new TimeSpan(0, 0, 5)
                );

            var builder = new DbCommandFactory();
            var cmd = builder.Instantiate(conn, query);
            Assert.That(cmd.CommandTimeout, Is.EqualTo(5));
        }

        [Test]
        public void Build_TimeoutSetToZero_TimeoutSet0Seconds()
        {
            var conn = new SqlConnection();
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == "Data Source=server;Initial Catalog=database;Integrated Security=SSPI"
                && x.Statement == "WAITFOR DELAY '00:00:15'"
                && x.Timeout == new TimeSpan(0, 0, 0)
                );

            var builder = new DbCommandFactory();
            var cmd = builder.Instantiate(conn, query);
            Assert.That(cmd.CommandTimeout, Is.EqualTo(0));
        }

        [Test]
        public void Build_TimeoutSetTo30_TimeoutSet30Seconds()
        {
            var conn = new SqlConnection();
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == "Data Source=server;Initial Catalog=database;Integrated Security=SSPI"
                && x.Statement == "WAITFOR DELAY '00:00:15'"
                && x.Timeout == new TimeSpan(0, 0, 30)
                );

            var builder = new DbCommandFactory();
            var cmd = builder.Instantiate(conn, query); Assert.That(cmd.CommandTimeout, Is.EqualTo(30));
        }
    }
}
