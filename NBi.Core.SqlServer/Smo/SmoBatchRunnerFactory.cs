using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Batch;
using System.Data.SqlClient;

namespace NBi.Core.SqlServer.Smo
{
    public class SmoBatchRunnerFactory : IBatchRunnerFatory
    {
        public IDecorationCommandImplementation Get(IBatchCommand command, IDbConnection connection)
        {
            if (command is IBatchRunCommand)
                return new BatchRunCommand(command as IBatchRunCommand, connection as SqlConnection);

            throw new ArgumentException();
        }
    }
}
