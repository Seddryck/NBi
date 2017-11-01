using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet.Resolver.Query;
using NBi.Xml.Items;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core.ResultSet.Resolver.Query
{
    [TestFixture]
    public class AssemblyQueryResolverTest
    {
        private AssemblyQueryResolverArgs BuildArgs()
        {
            return new AssemblyQueryResolverArgs(
                @"NBi.Testing.dll",
                "NBi.Testing.Acceptance.Resources.AssemblyClass",
                "GetTextSelectSql",
                false,
                new Dictionary<string, object>() { { "prefix", "CY" } },
                ConnectionStringReader.GetSqlClient(),
                new List<IQueryParameter>() { new QueryParameterXml() { Name="param", StringValue="10" } },
                new List<IQueryTemplateVariable>() { new QueryTemplateVariableXml() { Name = "operator", Value = "not in" } },
                10);
        }

        [Test]
        public void Execute_Args_CommandInstantiated()
        {
            var resolver = new AssemblyQueryResolver(BuildArgs());
            var cmd = resolver.Execute();

            Assert.That(cmd, Is.Not.Null);
        }

        [Test]
        public void Execute_Args_ConnectionStringAssigned()
        {
            var resolver = new AssemblyQueryResolver(BuildArgs());
            var cmd = resolver.Execute();

            Assert.That(cmd.Connection.ConnectionString, Is.Not.Null.And.Not.Empty);
            Assert.That(cmd.Connection.ConnectionString, Is.EqualTo(ConnectionStringReader.GetSqlClient()));
        }

        [Test]
        public void Execute_Args_CommandTextAssigned()
        {
            var resolver = new AssemblyQueryResolver(BuildArgs());
            var cmd = resolver.Execute();

            Assert.That(cmd.CommandText, Is.StringStarting("select 'CY 2005', 366"));
        }

        [Test]
        public void Execute_Args_ParametersAssigned()
        {
            var resolver = new AssemblyQueryResolver(BuildArgs());
            var cmd = resolver.Execute();

            Assert.That(cmd.Parameters, Has.Count.EqualTo(1));
        }

        [Test]
        public void Execute_InvalidPath_ThrowExternalDependency()
        {
            var args = new AssemblyQueryResolverArgs(
                @"Acceptance\Resources\NBi.Testing.dll",
                "NBi.Testing.Acceptance.Resources.AssemblyClass",
                "GetTextSelectSql",
                false,
                new Dictionary<string, object>() { { "prefix", "CY" } },
                ConnectionStringReader.GetSqlClient(),
                new List<IQueryParameter>() { new QueryParameterXml() { Name = "param", StringValue = "10" } },
                new List<IQueryTemplateVariable>() { new QueryTemplateVariableXml() { Name = "operator", Value = "not in" } },
                10);

            var resolver = new AssemblyQueryResolver(args);
            Assert.Throws<ExternalDependencyNotFoundException>(() => resolver.Execute());
        }

        [Test]
        public void Execute_InvalidAbsolutePath_MessageAbsolutePath()
        {
            var args = new AssemblyQueryResolverArgs(
                @"C:\NotExisting.dll",
                "NBi.Testing.Acceptance.Resources.AssemblyClass",
                "GetTextSelectSql",
                false,
                new Dictionary<string, object>() { { "prefix", "CY" } },
                ConnectionStringReader.GetSqlClient(),
                null, null, 10);

            var resolver = new AssemblyQueryResolver(args);
            var ex = Assert.Catch<ExternalDependencyNotFoundException>(() => resolver.Execute());
            Assert.That(ex.Message, Is.StringContaining(@"C:\NotExisting.dll"));
        }

    }
}
