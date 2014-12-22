using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Batch.SqlServer
{
    class BatchRunCommand : IDecorationCommandImplementation
    {
        private readonly string connectionString;
        private readonly string fullPath;

        public BatchRunCommand(IBatchRunCommand command, SqlConnection connection)
        {
            this.connectionString = connection.ConnectionString;
            this.fullPath = command.FullPath;
        }       

        public void Execute()
        {
            if (!File.Exists(fullPath))
                throw new ExternalDependencyNotFoundException(fullPath);

            var script = File.ReadAllText(fullPath);

            var server = new Server();
            server.ConnectionContext.ConnectionString = connectionString;
            server.ConnectionContext.ExecuteNonQuery(script);
        }
    }
}
