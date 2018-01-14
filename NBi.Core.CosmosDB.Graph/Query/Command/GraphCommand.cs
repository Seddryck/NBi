using Microsoft.Azure.Documents.Linq;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.Query.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Command
{
    class GraphCommand : ICommand
    {
        public object Implementation { get; }
        public object Client { get; }

        public GraphCommand(GremlinClient client, GremlinQuery query)
        {
            Client = client;
            Implementation = query;
        }
    }
}
