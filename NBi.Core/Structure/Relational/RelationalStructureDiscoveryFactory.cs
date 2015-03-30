﻿using NBi.Core.Structure.Relational.Builders;
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

        public StructureDiscoveryCommand Instantiate(Target target, TargetType type, IEnumerable<CaptionFilter> filters)
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
            switch (target)
            {
                case Target.Schemas:
                    return new SchemaDiscoveryCommandBuilder();
                case Target.Tables:
                    return new TableDiscoveryCommandBuilder();
                case Target.Columns:
                    return new ColumnDiscoveryCommandBuilder();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
