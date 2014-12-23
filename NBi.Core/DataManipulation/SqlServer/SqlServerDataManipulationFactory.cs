using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.DataManipulation.SqlServer
{
    class SqlServerDataManipulationFactory : IDataManipulationFactory
    {
        public IDecorationCommandImplementation Get(IDataManipulationCommand command, IDbConnection connection)
        {
            if (!(connection is SqlConnection) || connection == null)
                throw new ArgumentException("connection");

            if (command is ILoadCommand)
                return new BulkLoadCommand(command as ILoadCommand, connection as SqlConnection);
            if (command is IResetCommand)
                return new TruncateCommand(command as IResetCommand, connection as SqlConnection);

            throw new ArgumentException();
        }
    }
}
