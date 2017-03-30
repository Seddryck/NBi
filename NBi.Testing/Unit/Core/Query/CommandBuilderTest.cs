using NBi.Core.Query;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            var builder = new CommandBuilder();
            var cmd = builder.Build("Data Source=server;Initial Catalog=database;Integrated Security=SSPI", "WAITFOR DELAY '00:00:15'", null, null, 5000);
            Assert.That(cmd.CommandTimeout, Is.EqualTo(5));
        }

        [Test]
        public void Build_TimeoutSetToZero_TimeoutSet0Seconds()
        {
            var builder = new CommandBuilder();
            var cmd = builder.Build("Data Source=server;Initial Catalog=database;Integrated Security=SSPI", "WAITFOR DELAY '00:00:15'", null, null, 0);
            Assert.That(cmd.CommandTimeout, Is.EqualTo(0));
        }

        [Test]
        public void Build_TimeoutSetTo30_TimeoutSet30Seconds()
        {
            var builder = new CommandBuilder();
            var cmd = builder.Build("Data Source=server;Initial Catalog=database;Integrated Security=SSPI", "WAITFOR DELAY '00:00:15'", null, null, 30000);
            Assert.That(cmd.CommandTimeout, Is.EqualTo(30));
        }
    }
}
