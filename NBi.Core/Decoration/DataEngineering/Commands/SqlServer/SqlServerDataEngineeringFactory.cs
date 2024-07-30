using NBi.Core.Decoration.DataEngineering;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace NBi.Core.Decoration.DataEngineering.Commands.SqlServer
{
    class SqlServerDataEngineeringFactory : IDataEngineeringFactory
    {
        public IDecorationCommand Instantiate(IDataEngineeringCommandArgs args, IDbConnection connection)
        {
            if (!(connection is SqlConnection) || connection == null)
                throw new ArgumentException(nameof(connection));

            return args switch
            {
                TableLoadCommandArgs loadArgs => new BulkLoadCommand(loadArgs),
                TableTruncateCommandArgs resetArgs => new TruncateCommand(resetArgs),
                SqlBatchRunCommandArgs batchRunArgs => new BatchRunCommand(batchRunArgs),
                _ => throw new ArgumentException(),
            };
        }
    }
}
