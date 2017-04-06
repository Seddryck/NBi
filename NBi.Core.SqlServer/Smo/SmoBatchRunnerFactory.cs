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
            {
                if (connection == null)
                    throw new ArgumentNullException("connection");
                if (connection is SqlConnection)
                    throw new ArgumentException(
                        String.Format("To execute a SQL Batch on a SQL Server, you must provide a connection-string that is associated to a '{0}'. The connection-string '{1}' is associated to a '{2}'"
                            , typeof(SqlConnection).AssemblyQualifiedName
                            , connection.ConnectionString
                            , connection.GetType().AssemblyQualifiedName)
                        , "connection");
                return new BatchRunCommand(command as IBatchRunCommand, connection as SqlConnection);
            }
                
            throw new ArgumentException();
        }
    }
}
