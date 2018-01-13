using Microsoft.Azure.Documents.Linq;
using NBi.Core.CosmosDb.Graph.Query.Session;
using NBi.Core.Query.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Graph.Query.Command
{
    class GremlinCommand : ICommand
    {
        public object Implementation { get; }
        public object Session { get; }

        public GremlinCommand(CosmosDbSession session, CosmosDbQuery query)
        {
            Session = session;
            Implementation = query;
        }
    }
}
