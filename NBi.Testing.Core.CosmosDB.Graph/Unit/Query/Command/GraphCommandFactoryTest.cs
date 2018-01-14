using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBiSession = NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.CosmosDb.Query.Command;
using Microsoft.Azure.Documents.Linq;

namespace NBi.Testing.Core.CosmosDb.Query.Command
{
    public class GremlinCommandFactoryTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        [Test]
        public void CanHandle_GremlinSession_True()
        {
            var session = new GraphClient("xyz", "graphs", base64AuthKey, "db", "FoF");
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            Assert.That(factory.CanHandle(session), Is.True);
        }

        [Test]
        public void CanHandle_OtherKindOfSession_False()
        {
            var session = Mock.Of<NBiSession.IClient>();
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            Assert.That(factory.CanHandle(session), Is.False);
        }

        [Test]
        public void Instantiate_GremlinSessionAndQuery_CommandNotNull()
        {
            var session = new GraphClient("xyz", "graphs", base64AuthKey, "db", "FoF");
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            var command = factory.Instantiate(session, query);
            Assert.That(command, Is.Not.Null);
        }

        [Test]
        public void Instantiate_GremlinSessionAndQuery_CommandImplementationCorrectType()
        {
            var session = new GraphClient("xyz", "graphs", base64AuthKey, "db", "FoF");
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            var command = factory.Instantiate(session, query);
            var impl = command.Implementation;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.TypeOf<GremlinQuery>());
        }

        [Test]
        public void Instantiate_GremlinSessionAndQuery_SessionCorrectType()
        {
            var session = new GraphClient("xyz", "graphs", base64AuthKey, "db", "FoF");
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            var command = factory.Instantiate(session, query);
            var impl = command.Client;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.InstanceOf<GremlinClient>());
        }
    }
}
