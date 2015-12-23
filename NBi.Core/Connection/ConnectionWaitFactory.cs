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

            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.Get(command.ConnectionString);

            var implementation = new ConnectionWaitCommand(connection, command.TimeOut);
            return implementation;
        }
    }
}
