using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Configuration.Extension;
using NBi.Core.CosmosDb.Graph.Query.Execution;
using NBi.Core.CosmosDb.Graph.Query.Command;
using NBi.Core.CosmosDb.Graph.Query.Session;

namespace NBi.Testing.Core.CosmosDb.Graph.Unit.Configuration.Extension
{
    public class ExtensionAnalyzerTest
    {
        [Test]
        public void Execute_CosmosDbGraph_Three()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb.Graph");
            Assert.That(types.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Execute_CosmosDbGraph_ISessionFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb.Graph");
            Assert.That(types, Has.Member(typeof(GremlinSessionFactory)));
        }

        [Test]
        public void Execute_CosmosDbGraph_ICommandFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb.Graph");
            Assert.That(types, Has.Member(typeof(GremlinCommandFactory)));
        }

        [Test]
        public void Execute_CosmosDbGraph_IExecutionEngine()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb.Graph");
            Assert.That(types, Has.Member(typeof(GremlinExecutionEngine)));
        }
    }
}
