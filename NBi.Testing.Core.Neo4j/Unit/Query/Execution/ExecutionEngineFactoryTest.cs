using Moq;
using NBi.Core.Configuration;
using NBi.Core.Neo4j.Query.Command;
using NBi.Core.Neo4j.Query.Execution;
using NBi.Core.Neo4j.Query.Client;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Neo4j.Unit.Query.Execution
{
    public class ExecutionEngineFactoryTest
    {
        private class BoltConfig : IExtensionsConfiguration
        {
            public IReadOnlyCollection<Type> Extensions => new List<Type>()
            {
                typeof(BoltClientFactory),
                typeof(BoltCommandFactory),
                typeof(BoltExecutionEngine),
            };
        }

        [Test]
        public void Instantiate_BoltConnectionString_BoltExecutionEngine()
        {
            var config = new BoltConfig();
            var sessionProvider = new ClientProvider(config);
            var commandProvider = new CommandProvider(config);
            var factory = new ExecutionEngineFactory(sessionProvider, commandProvider, config);

            var query = Mock.Of<IQuery>
                (
                    x => x.ConnectionString == "bolt://login:passw0rd@host:7687/"
                    && x.Statement == "MATCH(actor: Person) RETURN actor.name"
                );

            var engine = factory.Instantiate(query);
            Assert.That(engine, Is.Not.Null);
            Assert.That(engine, Is.TypeOf<BoltExecutionEngine>());
        }
    }
}
