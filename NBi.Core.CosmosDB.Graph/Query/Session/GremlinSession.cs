using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Graphs.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Session
{
    class GremlinSession
    {
        public DocumentClient Client { get; }
        public string DatabaseId { get; }
        public string GraphId { get; }
        protected DocumentCollection Documents { get; private set; }

        public GremlinSession(Uri endpoint, string authKey, string databaseId, string graphId)
        {
            try
            { Client = new DocumentClient(endpoint, authKey); }
            catch (FormatException ex)
            { throw new NBiException($"The connectionString for CosmosDb is expecting an AuthKey encoded in base64. The value '{authKey}' is not a base64-encoded.", ex); }

            DatabaseId = databaseId ?? throw new NBiException($"The connectionString for CosmosDb is expecting a databaseId and this value cannot be null or empty");
            GraphId = graphId ?? throw new NBiException($"The connectionString for CosmosDb is expecting a graphId and this value cannot be null or empty");
        }

        public IDocumentQuery<dynamic> CreateCommand(string preparedStatement)
        {
            Initialize();
            return Client.CreateGremlinQuery<dynamic>(Documents, preparedStatement);
        }

        public dynamic[] Run(IDocumentQuery<dynamic> query)
        {
            var result = Task.Run(async () => await GetAllResultsAsync(query)).Result;
            return result;
        }

        private async static Task<T[]> GetAllResultsAsync<T>(IDocumentQuery<T> queryAll)
        {
            var list = new List<T>();
            while (queryAll.HasMoreResults)
            {
                var docs = await queryAll.ExecuteNextAsync<T>();
                foreach (var d in docs)
                    list.Add(d);
            }
            return list.ToArray();
        }

        protected void Initialize()
        {
            var getDatabaseTask = Task.Run(async () => await GetDatabaseAsync(Client, DatabaseId));
            getDatabaseTask.Wait();

            var getGraphTask = Task.Run(async () => await GetGraphAsync(Client, DatabaseId, GraphId));
            Documents = getGraphTask.Result;
        }

        private async Task<Database> GetDatabaseAsync(DocumentClient client, string databaseid)
        {
            try
            {
                return await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException documentClientException)
            {
                if (documentClientException.Error?.Code == "NotFound")
                    throw new NBiException($"The database '{DatabaseId}' does not exist.");
                else
                    throw;
            }
            throw new InvalidOperationException();
        }

        private async Task<DocumentCollection> GetGraphAsync(DocumentClient client, string databaseid, string graphId)
        {
            try
            {
                return await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, GraphId));
            }
            catch (DocumentClientException documentClientException)
            {
                if (documentClientException.Error?.Code == "NotFound")
                {
                    if (documentClientException.Error?.Code == "NotFound")
                        throw new NBiException($"The collection/graph '{GraphId}' does not exist.");
                    else
                        throw;
                }
            }
            throw new InvalidOperationException();
        }
    }
}
