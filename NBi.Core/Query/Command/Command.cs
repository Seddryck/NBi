using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Command;

class Command : ICommand
{
    public object Implementation { get; }
    public object Client { get; }

    public Command(IDbConnection connection, IDbCommand command)
    {
        Client = connection;
        Implementation = command;
    }
}
