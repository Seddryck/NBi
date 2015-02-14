using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Batch.SqlServer
{
    class SqlServerBatchFactory : IBatchFatory
    {
        public IDecorationCommandImplementation Get(IBatchCommand command, IDbConnection connection)
        {
            if (command is IBatchRunCommand)
                return new BatchRunCommand(command as IBatchRunCommand, connection as SqlConnection);

            throw new ArgumentException();
        }
    }
}
