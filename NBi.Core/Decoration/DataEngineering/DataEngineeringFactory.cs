using System;
using System.Data;
using System.Data.SqlClient;
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

                        switch (connection)
                        {
                            case SqlConnection sqlConnection: return new SqlServerDataEngineeringFactory().Instantiate(args, sqlConnection);
                            default: throw new ArgumentException();
                        }
                    }
            }
        }
    }
}
