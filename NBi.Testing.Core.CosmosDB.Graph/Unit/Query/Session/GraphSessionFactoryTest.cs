using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.CosmosDb.Query.Session;

namespace NBi.Testing.Core.CosmosDb.Unit.Query.Session
{
    public class GraphSessionFactoryTest
    {
        [Test]
        public void CanHandle_CosmosDbGraph_True()
        {
            var factory = new GraphSessionFactory();
            Assert.That(factory.CanHandle("Endpoint=https://xyz.graphs.azure.com:443;AuthKey=@uthk3y;database=db;graph=FoF"), Is.True);
        }

        [Test]
        public void CanHandle_OleDbConnectionString_False()
        {
            var factory = new GraphSessionFactory();
            Assert.That(factory.CanHandle("data source=SERVER;initial catalog=DB;IntegratedSecurity=true;Provider=OLEDB.1"), Is.False);
        }

        [Test]
        public void Instantiate_CosmosDbGraph_GremlinSession()
        {
            var factory = new GraphSessionFactory();
            var session = factory.Instantiate("Endpoint=https://xyz.graphs.azure.com:443;AuthKey=@uthk3y;database=db;graph=FoF");
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.TypeOf<GraphSession>());
        }

        
    }
}
