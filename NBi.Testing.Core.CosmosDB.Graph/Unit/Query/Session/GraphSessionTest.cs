using NBi.Core;
using NBi.Core.CosmosDb.Query.Session;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.CosmosDb.Unit.Query.Session
{
    public class GraphSessionTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        [Test]
        public void InstantiateUnderlyingSession_CosmosDbConnectionString_ISession()
        {
            var factory = new GraphSessionFactory();
            var session = factory.Instantiate($"Endpoint=https://xyz.graphs.azure.com:443;AuthKey={base64AuthKey};database=db;graph=FoF");
            Assert.That(session.UnderlyingSessionType, Is.EqualTo(typeof(GremlinSession)));
        }

        [Test]
        public void InstantiateCreate_CosmosDbConnectionString_ISession()
        {
            var factory = new GraphSessionFactory();
            var session = factory.Instantiate($"Endpoint=https://xyz.graphs.azure.com:443;AuthKey={base64AuthKey};database=db;graph=FoF");
            var underlyingSession = session.CreateNew();
            Assert.That(underlyingSession, Is.Not.Null);
            Assert.That(underlyingSession, Is.AssignableTo<GremlinSession>());
        }

        [Test]
        public void InstantiateCreate_CosmosDbConnectionStringWithoutBase64Encoding_NBiException()
        {
            var factory = new GraphSessionFactory();
            var session = factory.Instantiate($"Endpoint=https://xyz.graphs.azure.com:443;AuthKey=@uthK3y;database=db;graph=FoF");
            Assert.Throws<NBiException>( () => session.CreateNew());
        }
    }
}
