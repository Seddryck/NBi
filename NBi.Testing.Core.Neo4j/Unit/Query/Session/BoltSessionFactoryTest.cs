using NBi.Core.Neo4j.Query.Session;
using NUnit.Framework;
using Driver = Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Neo4j.Unit.Query.Session
{
    public class BoltSessionFactoryTest
    {
        [Test]
        public void CanHandle_BoltUrl_True()
        {
            var factory = new BoltSessionFactory();
            Assert.That(factory.CanHandle("bolt://login:password@host:port/"), Is.True);
        }

        [Test]
        public void CanHandle_OleDbConnectionString_False()
        {
            var factory = new BoltSessionFactory();
            Assert.That(factory.CanHandle("data source=SERVER;initial catalog=DB;IntegratedSecurity=true;Provider=OLEDB.1"), Is.False);
        }

        [Test]
        public void Instantiate_BoltUrl_BoltSession()
        {
            var factory = new BoltSessionFactory();
            var session = factory.Instantiate("bolt://login:password@host:7687/");
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.TypeOf<BoltSession>());
        }

        
    }
}
