using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using NBi.Core.Decoration.DataEngineering;
using NBi.Core.Decoration.DataEngineering.Commands;
using NBi.Core.Decoration.DataEngineering.Commands.SqlServer;
using NBi.Core.Query.Client;

namespace NBi.Core.Decoration.DataEngineering
{
    public class DataEngineeringFactory
    {
        public IDecorationCommand Instantiate(IDataEngineeringCommandArgs args)
        {
            switch (args)
            {
                case ConnectionWaitCommandArgs connectionWaitArgs: return new ConnectionWaitCommand(connectionWaitArgs);
                case EtlRunCommandArgs etlRunArgs: return new EtlRunCommand(etlRunArgs);
                default:
                    {
                        var sessionFactory = new ClientProvider();
                        var connection = sessionFactory.Instantiate(args.ConnectionString).CreateNew() as IDbConnection;

                        return connection switch
                        {
                            SqlConnection sqlConnection => new SqlServerDataEngineeringFactory().Instantiate(args, sqlConnection),
                            _ => throw new ArgumentException(),
                        };
                    }
            }
        }
    }
}
