using NBi.Core.Structure.Relational.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Relational
{
    class RelationalStructureDiscoveryFactory : IStructureDiscoveryFactory
    {
        private readonly IDbConnection connection;
        public RelationalStructureDiscoveryFactory(IDbConnection connection)
        {
            this.connection = connection as SqlConnection;
        }

        public StructureDiscoveryCommand Instantiate(Target target, TargetType type, IEnumerable<IFilter> filters)
        {
            var builder = InstantiateBuilder(target);
            builder.Build(filters);

            var cmd = connection.CreateCommand();
            cmd.CommandText = builder.GetCommandText();
            var postFilters = builder.GetPostFilters();

            var description = new CommandDescription(target, filters);

            RelationalCommand command = null;
            command = new RelationalCommand(cmd, postFilters, description);

            return command;
        }


        protected virtual IDiscoveryCommandBuilder InstantiateBuilder(Target target)
        {
            switch (target)
            {
                case Target.Perspectives:
                    return new SchemaDiscoveryCommandBuilder();
                case Target.Tables:
                    return new TableDiscoveryCommandBuilder();
                case Target.Columns:
                    return new ColumnDiscoveryCommandBuilder();
                case Target.Routines:
                    return new RoutineDiscoveryCommandBuilder();
                case Target.Parameters:
                    return new RoutineParameterDiscoveryCommandBuilder();
                default:
                    throw new ArgumentOutOfRangeException(string.Format("The value '{0}' is not supported when instantiating with 'RelationalStructureDiscoveryFactory'.", target));
            }
        }

    }
}
