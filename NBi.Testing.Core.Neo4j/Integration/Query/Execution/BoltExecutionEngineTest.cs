using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NBi.Xml.Items;
using NUnit.Framework;
using NBi.Core.Neo4j.Query.Command;
using NBi.Core.Neo4j.Query.Execution;
using Neo4j.Driver.V1;
using System.Data;

namespace NBi.Testing.Core.Neo4j.Integration.Query.Execution
{
    [TestFixture]
    public class BoltExecutionEngineTest
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
        public void Execute_ValidStatement_DataSetFilled()
        {
            var session = GraphDatabase.Driver($"bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "bolt")).Session();
            var statement = new Statement("MATCH (tom:Person {name: \"Tom Hanks\"})-[:ACTED_IN]->(tomHanksMovies) WHERE tomHanksMovies.released>2000 RETURN tomHanksMovies.title, tomHanksMovies.released");

            var engine = new BoltExecutionEngine(session, statement);
            var ds = engine.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Columns, Has.Count.EqualTo(2));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(4));

            var titles = new List<object>();
            var years = new List<object>();
            foreach (DataRow row in ds.Tables[0].Rows)
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (row.Table.Columns[i].ColumnName == "tomHanksMovies.title")
                        titles.Add(row.ItemArray[i]);
                    else
                        years.Add(row.ItemArray[i]);
                }

            var expectedTitles = new[] { "Charlie Wilson's War", "The Polar Express", "The Da Vinci Code", "Cloud Atlas" };
            foreach (var expectedTitle in expectedTitles)
                Assert.That(titles, Has.Member(expectedTitle));

            var expectedYears = new[] { 2007, 2012, 2004, 2006 };
            foreach (var expectedYear in expectedYears)
                Assert.That(years, Has.Member(expectedYear));
        }

        [Test]
        public void ExecuteScalar_ValidStatement_ValueReturned()
        {
            var session = GraphDatabase.Driver($"bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "bolt")).Session();
            var statement = new Statement("MATCH (tom:Person {name: \"Tom Hanks\"}) RETURN tom.born");

            var engine = new BoltExecutionEngine(session, statement);
            var value = engine.ExecuteScalar();
            Assert.That(value, Is.EqualTo(1956));
        }

        [Test]
        public void ExecuteScalar_StatementWithoutResult_NullReturned()
        {
            var session = GraphDatabase.Driver($"bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "bolt")).Session();
            var statement = new Statement("MATCH (tom:Person {name: \"Cédric Charlier\"}) RETURN tom.born");

            var engine = new BoltExecutionEngine(session, statement);
            var value = engine.ExecuteScalar();
            Assert.That(value, Is.EqualTo(null));
        }

        [Test]
        public void ExecuteList_Statement_ListReturned()
        {
            var session = GraphDatabase.Driver($"bolt://127.0.0.1:7687", AuthTokens.Basic("neo4j", "bolt")).Session();
            var statement = new Statement("MATCH (actor:Person) RETURN actor.name");

            var engine = new BoltExecutionEngine(session, statement);
            var list = engine.ExecuteList<string>();
            Assert.That(list, Has.Member("Tom Hanks"));
            Assert.That(list, Has.Member("Keanu Reeves"));
        }
    }
}
