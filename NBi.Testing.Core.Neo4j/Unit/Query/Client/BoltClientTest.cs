using NBi.Core.Neo4j.Query.Client;
using NUnit.Framework;
using Driver = Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Neo4j.Unit.Query.Client
{
    public class BoltClientTest
    {
        [Test]
        public void InstantiateUnderlyingSession_BoltUrl_ISession()
        {
            var factory = new BoltClientFactory();
            var session = factory.Instantiate("bolt://login:password@host:7687/");
            Assert.That(session.UnderlyingSessionType, Is.EqualTo(typeof(Driver.ISession)));
        }

        [Test]
        public void InstantiateCreate_BoltUrl_ISession()
        {
            var factory = new BoltClientFactory();
            var session = factory.Instantiate("bolt://login:password@host:7687/");
            var underlyingSession = session.CreateNew();
            Assert.That(underlyingSession, Is.Not.Null);
            Assert.That(underlyingSession, Is.AssignableTo<Driver.ISession>());
        }
    }
}
