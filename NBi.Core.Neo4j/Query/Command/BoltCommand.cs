using NBi.Core.Query.Command;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Neo4j.Query.Command
{
    class BoltCommand : ICommand
    {
        public object Implementation { get; }
        public object Client { get; }

        public BoltCommand(ISession session, Statement implementation)
        {
            Client = session;
            Implementation = implementation;
        }
    }
}
