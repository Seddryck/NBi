using Moq;
using NBi.Core.Configuration;
using NBi.Core.CosmosDb.Graph.Query.Command;
using NBi.Core.CosmosDb.Graph.Query.Execution;
using NBi.Core.CosmosDb.Graph.Query.Session;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Session;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.CosmosDb.Graph.Unit.Query.Execution
{
    public class ExecutionEngineFactoryTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        private class GremlinConfig : IExtensionsConfiguration
        {
            public IReadOnlyCollection<Type> Extensions => new List<Type>()
            {
                typeof(GremlinSessionFactory),
                typeof(GremlinCommandFactory),
                typeof(GremlinExecutionEngine),
            };
        }

        [Test]
        public void Instantiate_GremlinConnectionString_GremlinExecutionEngine()
        {
            var config = new GremlinConfig();
            var sessionProvider = new SessionProvider(config);
            var commandProvider = new CommandProvider(config);
            var factory = new ExecutionEngineFactory(sessionProvider, commandProvider, config);

            var query = Mock.Of<IQuery>
                (
                    x => x.ConnectionString == $"Endpoint=https://xyz.graphs.azure.com:443;AuthKey={base64AuthKey};database=db;graph=FoF"
                    && x.Statement == "g.V().Count()"
                );

            var engine = factory.Instantiate(query);
            Assert.That(engine, Is.Not.Null);
            Assert.That(engine, Is.TypeOf<GremlinExecutionEngine>());
        }
    }
}
