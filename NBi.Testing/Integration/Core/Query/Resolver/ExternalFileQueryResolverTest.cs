using NBi.Core;
using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Xml.Items;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Testing.Integration.Core.Query.Resolver
{
    [TestFixture]
    public class ExternalFileQueryResolverTest
    {
        private ExternalFileQueryResolverArgs BuildArgs()
        {
            return new ExternalFileQueryResolverArgs(
                @"Integration\Core\Resources\query.sql",
                ConnectionStringReader.GetSqlClient(),
                new List<IQueryParameter>() { new QueryParameter("param", "10") },
                new List<IQueryTemplateVariable>() { new QueryTemplateVariableXml() { Name = "operator", Value = "not in" } },
                new TimeSpan(0, 0, 10));
        }

        [Test]
        public void Execute_Args_CommandInstantiated()
        {
            var resolver = new ExternalFileQueryResolver(BuildArgs());
            var cmd = resolver.Execute();

            Assert.That(cmd, Is.Not.Null);
        }

        [Test]
        public void Execute_Args_ConnectionStringAssigned()
        {
            var resolver = new ExternalFileQueryResolver(BuildArgs());
            var query = resolver.Execute();

            Assert.That(query.ConnectionString, Is.Not.Null.And.Not.Empty);
            Assert.That(query.ConnectionString, Is.EqualTo(ConnectionStringReader.GetSqlClient()));
        }

        [Test]
        public void Execute_Args_CommandTextAssigned()
        {
            var resolver = new ExternalFileQueryResolver(BuildArgs());
            var query = resolver.Execute();

            Assert.That(query.Statement, Is.EqualTo("select * from myTable;"));
        }

        [Test]
        public void Execute_Args_ParametersAssigned()
        {
            var resolver = new ExternalFileQueryResolver(BuildArgs());
            var query = resolver.Execute();

            Assert.That(query.Parameters, Has.Count.EqualTo(1));
        }

        [Test]
        public void Execute_InvalidPath_ThrowExternalDependency()
        {
            var args = new ExternalFileQueryResolverArgs(
                @"NotExistingFile.sql",
                ConnectionStringReader.GetSqlClient(),
                new List<IQueryParameter>() { new QueryParameter("param", "10") },
                new List<IQueryTemplateVariable>() { new QueryTemplateVariableXml() { Name = "operator", Value = "not in" } },
                new TimeSpan(0, 0, 10));
            var resolver = new ExternalFileQueryResolver(args);
            Assert.Throws<ExternalDependencyNotFoundException>(() => resolver.Execute());
        }

        [Test]
        public void Execute_InvalidPath_MessageRelativePath()
        {
            var args = new ExternalFileQueryResolverArgs(
                @"NotExistingFile.sql",
                ConnectionStringReader.GetSqlClient(),
                new List<IQueryParameter>() { new QueryParameter("param", "10") },
                new List<IQueryTemplateVariable>() { new QueryTemplateVariableXml() { Name = "operator", Value = "not in" } },
                new TimeSpan(0, 0, 10));
            var resolver = new ExternalFileQueryResolver(args);
            var ex = Assert.Catch<ExternalDependencyNotFoundException>(() => resolver.Execute());
            Assert.That(ex.Message, Is.StringContaining(@"NBi.Testing\bin\"));
            Assert.That(ex.Message, Is.StringContaining(@"NotExistingFile.sql"));
        }

        [Test]
        public void Execute_InvalidAbsolutePath_MessageAbsolutePath()
        {
            var args = new ExternalFileQueryResolverArgs(
                @"C:\NotExistingFile.sql",
                ConnectionStringReader.GetSqlClient(),
                new List<IQueryParameter>() { new QueryParameter("param", "10") },
                new List<IQueryTemplateVariable>() { new QueryTemplateVariableXml() { Name = "operator", Value = "not in" } },
                new TimeSpan(0, 0, 10));
            var resolver = new ExternalFileQueryResolver(args);
            var ex = Assert.Catch<ExternalDependencyNotFoundException>(() => resolver.Execute());
            Assert.That(ex.Message, Is.StringContaining(@"C:\NotExistingFile.sql"));
        }

    }
}
