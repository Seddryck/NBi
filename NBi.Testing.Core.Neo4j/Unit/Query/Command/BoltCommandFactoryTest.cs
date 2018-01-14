using NBi.Core.Neo4j.Query.Client;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBiSession = NBi.Core.Query.Client;
using Driver = Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using NBi.Core.Neo4j.Query.Command;

namespace NBi.Testing.Core.Neo4j.Query.Command
{
    public class BoltCommandFactoryTest
    {
        [Test]
        public void CanHandle_BoltSession_True()
        {
            var session = new BoltClient("host", "7687", "login", "p@ssw0rd");
            var query = Mock.Of<IQuery>();
            var factory = new BoltCommandFactory();
            Assert.That(factory.CanHandle(session), Is.True);
        }

        [Test]
        public void CanHandle_OtherKindOfSession_False()
        {
            var session = Mock.Of<NBiSession.IClient>();
            var query = Mock.Of<IQuery>();
            var factory = new BoltCommandFactory();
            Assert.That(factory.CanHandle(session), Is.False);
        }

        [Test]
        public void Instantiate_BoltSession_NotNull()
        {
            var session = new BoltClient("host", "7687", "login", "p@ssw0rd");
            var query = Mock.Of<IQuery>();
            var factory = new BoltCommandFactory();
            var command = factory.Instantiate(session, query);
            Assert.That(command, Is.Not.Null);
        }

        [Test]
        public void Instantiate_BoltSession_ImplementationCorrectType()
        {
            var session = new BoltClient("host", "7687", "login", "p@ssw0rd");
            var query = Mock.Of<IQuery>();
            var factory = new BoltCommandFactory();
            var command = factory.Instantiate(session, query);
            var impl = command.Implementation;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.TypeOf<Driver.Statement>());
        }

        [Test]
        public void Instantiate_BoltSession_SessionCorrectType()
        {
            var session = new BoltClient("host", "7687", "login", "p@ssw0rd");
            var query = Mock.Of<IQuery>();
            var factory = new BoltCommandFactory();
            var command = factory.Instantiate(session, query);
            var impl = command.Client;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.InstanceOf<Driver.ISession>());
        }
    }
}
