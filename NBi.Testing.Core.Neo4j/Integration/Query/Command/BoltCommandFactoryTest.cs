using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NBi.Xml.Items;
using NUnit.Framework;
using NBi.Core.Scalar.Resolver;
using Moq;
using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using System.Data.Common;
using NBi.Core.Neo4j.Query.Client;
using Driver = Neo4j.Driver.V1;
using NBi.Core.Neo4j.Query.Command;

namespace NBi.Testing.Core.Neo4j.Integration.Query.Command
{
    [TestFixture]
    public class BoltCommandTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void Instantiate_NoParameter_CorrectResultSet()
        {
            var conn = new BoltClient("127.0.0.1", "7687", "neo4j", "bolt");
            var query = Mock.Of<IQuery>(
                x => x.Statement == "MATCH (tom:Person {name: \"Tom Hanks\"})-[:ACTED_IN]->(tomHanksMovies) WHERE tomHanksMovies.released>2000 RETURN tomHanksMovies.title, tomHanksMovies.released"
                );
            var factory = new BoltCommandFactory();
            var cmd = factory.Instantiate(conn, query).Implementation;
            var statement = cmd as Driver.Statement;

            var session = conn.CreateSession();
            var results = session.Run(statement);

            Assert.That(results.Count, Is.EqualTo(4));
            Assert.That(results.Keys.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Instantiate_OneParameterWithTypeString_CorrectResultSet()
        {
            var conn = new BoltClient("127.0.0.1", "7687", "neo4j", "bolt");
            var paramActor = new Mock<IQueryParameter>();
            paramActor.SetupGet(x => x.Name).Returns("actorName");
            paramActor.Setup(x => x.GetValue()).Returns("Tom Hanks");

            var query = Mock.Of<IQuery>(
                x => x.Statement == "MATCH (tom:Person {name: $actorName})-[:ACTED_IN]->(tomHanksMovies) WHERE tomHanksMovies.released>2000 RETURN tomHanksMovies.title, tomHanksMovies.released"
                && x.Parameters == new List<IQueryParameter>() { paramActor.Object }
                );
            var factory = new BoltCommandFactory();
            var cmd = factory.Instantiate(conn, query).Implementation;
            var statement = cmd as Driver.Statement;

            var session = conn.CreateSession();
            var results = session.Run(statement);

            Assert.That(results.Count, Is.EqualTo(4));
            Assert.That(results.Keys.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Instantiate_OneParameterWithTypeStringWithDollarSymbol_CorrectResultSet()
        {
            var conn = new BoltClient("127.0.0.1", "7687", "neo4j", "bolt");
            var paramActor = new Mock<IQueryParameter>();
            paramActor.SetupGet(x => x.Name).Returns("$actorName");
            paramActor.Setup(x => x.GetValue()).Returns("Tom Hanks");

            var query = Mock.Of<IQuery>(
                x => x.Statement == "MATCH (tom:Person {name: $actorName})-[:ACTED_IN]->(tomHanksMovies) WHERE tomHanksMovies.released>2000 RETURN tomHanksMovies.title, tomHanksMovies.released"
                && x.Parameters == new List<IQueryParameter>() { paramActor.Object }
                );
            var factory = new BoltCommandFactory();
            var cmd = factory.Instantiate(conn, query).Implementation;
            var statement = cmd as Driver.Statement;

            var session = conn.CreateSession();
            var results = session.Run(statement);

            Assert.That(results.Count, Is.EqualTo(4));
            Assert.That(results.Keys.Count(), Is.EqualTo(2));
        }

        [Test]
        //[Ignore("Neo4j extension doesn't support non-string parameters at the moment.")]
        public void Instantiate_OneParameterWithTypeInt_CorrectResultSet()
        {
            var conn = new BoltClient("127.0.0.1", "7687", "neo4j", "bolt");
            var paramReleased = new QueryParameter("movieReleased", "Integer", new LiteralScalarResolver<object>(new LiteralScalarResolverArgs("2000")));

            var query = Mock.Of<IQuery>(
                x => x.Statement == "MATCH (tom:Person {name: \"Tom Hanks\"})-[:ACTED_IN]->(tomHanksMovies) WHERE tomHanksMovies.released>$movieReleased RETURN tomHanksMovies.title, tomHanksMovies.released"
                && x.Parameters == new List<IQueryParameter>() { paramReleased }
                );
            var factory = new BoltCommandFactory();
            var cmd = factory.Instantiate(conn, query).Implementation;
            var statement = cmd as Driver.Statement;

            var session = conn.CreateSession();
            var results = session.Run(statement);

            Assert.That(results.Count, Is.EqualTo(4));
            Assert.That(results.Keys.Count(), Is.EqualTo(2));
        }
    }
}
