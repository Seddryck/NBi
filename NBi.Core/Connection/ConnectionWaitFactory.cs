using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Connection
{
    class ConnectionWaitFactory
    {
        public IDecorationCommandImplementation Get(IConnectionWaitCommand command)
        {

            var implementation = new ConnectionWaitCommand(command.ConnectionString, command.TimeOut);
            return implementation;
        }
    }
}
