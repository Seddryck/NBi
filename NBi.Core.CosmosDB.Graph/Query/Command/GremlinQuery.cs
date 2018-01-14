using Microsoft.Azure.Documents.Linq;
using NBi.Core.CosmosDb.Query.Client;
using Microsoft.Azure.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs.Elements;

namespace NBi.Core.CosmosDb.Query.Command
{
    class GremlinQuery
    {
        public string PreparedStatement { get; }
        public GremlinClient Session { get; }
        public GremlinQuery(GremlinClient session, string preparedStatement)
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
