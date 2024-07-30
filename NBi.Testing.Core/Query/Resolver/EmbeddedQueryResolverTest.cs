using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using Moq;
using NBi.Testing;

namespace NBi.Core.Testing.Query.Resolver
{
    [TestFixture]
    public class EmbeddedQueryResolverTest
    {
        private EmbeddedQueryResolverArgs BuildArgs()
        {
            return new EmbeddedQueryResolverArgs(
                "select * from myTable;",
                ConnectionStringReader.GetSqlClient(),
                [new QueryParameter("param", "10")],
                [Mock.Of<IQueryTemplateVariable>(x => x.Name == "operator" && x.Value == "not in")],
                new TimeSpan(0, 0, 10));
        }

        [Test]
        public void Execute_Args_CommandInstantiated()
        {
            var resolver = new EmbeddedQueryResolver(BuildArgs());
            var cmd = resolver.Execute();

            Assert.That(cmd, Is.Not.Null);
        }

        [Test]
        public void Execute_Args_ConnectionStringAssigned()
        {
            var resolver = new EmbeddedQueryResolver(BuildArgs());
            var query = resolver.Execute();

            Assert.That(query.ConnectionString, Is.Not.Null.And.Not.Empty);
            Assert.That(query.ConnectionString, Is.EqualTo(ConnectionStringReader.GetSqlClient()));
        }

        [Test]
        public void Execute_Args_CommandTextAssigned()
        {
            var resolver = new EmbeddedQueryResolver(BuildArgs());
            var query = resolver.Execute();

            Assert.That(query.Statement, Is.EqualTo("select * from myTable;"));
        }

        [Test]
        public void Execute_Args_ParametersAssigned()
        {
            var resolver = new EmbeddedQueryResolver(BuildArgs());
            var cmd = resolver.Execute();

            Assert.That(cmd.Parameters, Has.Count.EqualTo(1));
        }
        
    }
}
