using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NUnit.Framework;
using Moq;
using NBi.Core.CosmosDb.Graph.Query.Session;
using NBi.Core.CosmosDb.Graph.Query.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NBi.Testing.Core.CosmosDb.Graph.Integration.Query.Command
{
    [TestFixture]
    public class GremlinCommandTest
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
            GremlinSession conn = new GremlinSessionFactory().Instantiate(ConnectionStringReader.GetAzureGraph()) as GremlinSession;
            var query = Mock.Of<IQuery>(
                x => x.Statement == "g.V()"
                );
            var factory = new GremlinCommandFactory();
            var cosmosdbQuery = (factory.Instantiate(conn, query).Implementation) as CosmosDbQuery;
            var statement = cosmosdbQuery.Create();

            var session = conn.CreateCosmosDbSession();
            var results = session.Run(statement);
            Assert.That(results.Count, Is.EqualTo(4));
        }
    }
}
