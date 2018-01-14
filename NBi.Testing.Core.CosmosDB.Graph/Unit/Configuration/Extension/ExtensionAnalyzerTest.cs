using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Configuration.Extension;
using NBi.Core.CosmosDb.Query.Execution;
using NBi.Core.CosmosDb.Query.Command;
using NBi.Core.CosmosDb.Query.Session;

namespace NBi.Testing.Core.CosmosDb.Unit.Configuration.Extension
{
    public class ExtensionAnalyzerTest
    {
        [Test]
        public void Execute_CosmosDbGraph_Three()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Execute_CosmosDbGraph_ISessionFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types, Has.Member(typeof(GraphSessionFactory)));
        }

        [Test]
        public void Execute_CosmosDbGraph_ICommandFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types, Has.Member(typeof(GraphCommandFactory)));
        }

        [Test]
        public void Execute_CosmosDbGraph_IExecutionEngine()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.CosmosDb");
            Assert.That(types, Has.Member(typeof(GraphExecutionEngine)));
        }
    }
}
