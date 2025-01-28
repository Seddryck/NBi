using NBi.Core.Structure.Relational.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational;

class RelationalStructureDiscoveryFactory : IStructureDiscoveryFactory
{
    private readonly IDbConnection connection;
    public RelationalStructureDiscoveryFactory(IDbConnection connection)
    {
        this.connection = connection as IDbConnection;
    }

    public StructureDiscoveryCommand Instantiate(Target target, TargetType type, IEnumerable<IFilter> filters)
    {
        var builder = InstantiateBuilder(target);
        builder.Build(filters);

        var cmd = connection.CreateCommand();
        cmd.CommandText = builder.GetCommandText();
        var postFilters = builder.GetPostFilters();

        var description = new CommandDescription(target, filters);

        var command = new RelationalCommand(cmd, postFilters, description);

        return command;
    }


    protected virtual IDiscoveryCommandBuilder InstantiateBuilder(Target target)
    {
        return target switch
        {
            Target.Perspectives => new SchemaDiscoveryCommandBuilder(),
            Target.Tables => new TableDiscoveryCommandBuilder(),
            Target.Columns => new ColumnDiscoveryCommandBuilder(),
            Target.Routines => new RoutineDiscoveryCommandBuilder(),
            Target.Parameters => new RoutineParameterDiscoveryCommandBuilder(),
            _ => throw new ArgumentOutOfRangeException(string.Format("The value '{0}' is not supported when instantiating with 'RelationalStructureDiscoveryFactory'.", target)),
        };
    }

}
