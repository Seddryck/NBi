using Microsoft.Azure.Documents.Linq;
using NBi.Core.CosmosDb.Graph.Query.Session;
using Microsoft.Azure.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs.Elements;

namespace NBi.Core.CosmosDb.Graph.Query.Command
{
    class CosmosDbQuery
    {
        public string PreparedStatement { get; }
        public CosmosDbSession Session { get; }
        public CosmosDbQuery(CosmosDbSession session, string preparedStatement)
        {
            Session = session;
            PreparedStatement = preparedStatement;
        }

        public IDocumentQuery<dynamic> Create()
        {
            return Session.CreateCommand(PreparedStatement);
        }
    }
}
