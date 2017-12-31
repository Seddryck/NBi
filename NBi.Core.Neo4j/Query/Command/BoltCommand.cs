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
        public object Session { get; }

        public BoltCommand(ISession session, Statement implementation)
        {
            Session = session;
            Implementation = implementation;
        }
    }
}
