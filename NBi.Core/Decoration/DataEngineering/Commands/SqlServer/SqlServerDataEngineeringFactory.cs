using NBi.Core.Decoration.DataEngineering;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.Decoration.DataEngineering.Commands.SqlServer
{
    class SqlServerDataEngineeringFactory : IDataEngineeringFactory
    {
        public IDecorationCommand Instantiate(IDataEngineeringCommandArgs args, IDbConnection connection)
        {
            if (!(connection is SqlConnection) || connection == null)
                throw new ArgumentException(nameof(connection));

            switch (args)
            {
                case ILoadCommandArgs loadArgs: return new BulkLoadCommand(loadArgs);
                case IResetCommandArgs resetArgs: return new TruncateCommand(resetArgs);
                case IBatchRunCommandArgs batchRunArgs: return new BatchRunCommand(batchRunArgs);
                default: throw new ArgumentException();
            }
        }
    }
}
