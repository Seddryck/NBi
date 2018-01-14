using NBi.Core.Neo4j.Query.Command;
using NBi.Core.Neo4j.Query.Client;
using NBi.Core.Neo4j.Query.Execution;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Configuration.Extension;

namespace NBi.Testing.Core.Neo4j.Unit.Configuration.Extension
{
    public class ExtensionAnalyzerTest
    {
        [Test]
        public void Execute_Neo4j_Three()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Neo4j");
            Assert.That(types.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Execute_Neo4j_ISessionFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Neo4j");
            Assert.That(types, Has.Member(typeof(BoltClientFactory)));
        }

        [Test]
        public void Execute_Neo4j_ICommandFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Neo4j");
            Assert.That(types, Has.Member(typeof(BoltCommandFactory)));
        }

        [Test]
        public void Execute_Neo4j_IExecutionEngine()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Neo4j");
            Assert.That(types, Has.Member(typeof(BoltExecutionEngine)));
        }
    }
}
