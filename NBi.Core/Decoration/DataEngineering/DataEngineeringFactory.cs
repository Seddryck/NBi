using Microsoft.Data.SqlClient;
using NBi.Core.Decoration.DataEngineering.Commands;
using NBi.Core.Decoration.DataEngineering.Commands.SqlServer;
using NBi.Core.Query.Client;
using NBi.Extensibility.Query;
using System.Data;

namespace NBi.Core.Decoration.DataEngineering;

public class DataEngineeringFactory
{
    public IDecorationCommand Instantiate(IDataEngineeringCommandArgs args)
        => args switch
        {
            ConnectionWaitCommandArgs wait => new ConnectionWaitCommand(wait),
            EtlRunCommandArgs etl => new EtlRunCommand(etl),
            _ => Instantiate(args, new ClientProvider().Instantiate(args.ConnectionString)),
        };

    internal IDecorationCommand Instantiate(IDataEngineeringCommandArgs args, IClient client)
    {
        using var connection = client.CreateNew() as IDbConnection;
        if (args is TableLoadCommandArgs load && client is DubUrlClient dubUrl)
        {
            // DubUrl's SQL Server writer uses different locking and transaction options.
            if (connection is SqlConnection sql)
                return new BulkLoadCommand(load, sql.ConnectionString);
            if (dubUrl.Url.BulkCopyProvider is not null)
                return new DubUrlBulkLoadCommand(load, dubUrl.Url);
        }

        return connection switch
        {
            SqlConnection sqlConnection => new SqlServerDataEngineeringFactory().Instantiate(args, sqlConnection),
            _ => throw new ArgumentException("No data engineering command supports the selected connection."),
        };
    }
}
