using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.CosmosDb
{
    [TestFixture]
    public class TestSuiteSetup
    {
        private string[] Statements
        {
            get => new[]
{
            "g.V().Drop()"
            , "g.addV('person').property('id', 'thomas').property('firstName', 'Thomas').property('age', 44)"
            , "g.addV('person').property('id', 'mary').property('firstName', 'Mary').property('lastName', 'Andersen').property('age', 39)"
            , "g.addV('person').property('id', 'ben').property('firstName', 'Ben').property('lastName', 'Miller')"
            , "g.addV('person').property('id', 'robin').property('firstName', 'Robin').property('lastName', 'Wakefield')"
            , "g.V('thomas').addE('knows').to(g.V('mary'))"
            , "g.V('thomas').addE('knows').to(g.V('ben'))"
            , "g.V('ben').addE('knows').to(g.V('robin'))"
        };
        }

        [TestFixtureSetUp]
        public void Init()
        {
            var csBuilder = new DbConnectionStringBuilder() { ConnectionString = ConnectionStringReader.GetAzureGraph() };
            var endpoint = new Uri(csBuilder["endpoint"].ToString());
            var authKey = csBuilder["authkey"].ToString();
            var databaseId = csBuilder["database"].ToString();
            var collectionId = csBuilder["graph"].ToString();

            using (var client = new DocumentClient(endpoint, authKey))
            {
                var databaseUri = UriFactory.CreateDatabaseUri(databaseId);
                var database = client.ReadDatabaseAsync(databaseUri).Result;

                var collection = client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection() { Id = collectionId }).Result;

                var queryCheck = client.CreateGremlinQuery<dynamic>(collection, "g.V().Count()");
                var count = queryCheck.ExecuteNextAsync().Result.First();

                if (count != 4)
                {
                    foreach (var statement in Statements)
                    {
                        var query = client.CreateGremlinQuery<dynamic>(collection, statement);
                        var feed = query.ExecuteNextAsync().Result;
                        Console.WriteLine(feed.ToString());
                    }
                }
            }


        }
    }
}
