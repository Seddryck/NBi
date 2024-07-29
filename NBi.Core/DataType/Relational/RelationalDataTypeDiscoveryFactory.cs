using NBi.Core.DataType.Relational.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType.Relational
{
    class RelationalDataTypeDiscoveryFactory : IDataTypeDiscoveryFactory
    {
        private readonly IDbConnection connection;
        public RelationalDataTypeDiscoveryFactory(IDbConnection connection)
        {
            this.connection = (SqlConnection)connection;
        }

        public IDataTypeDiscoveryCommand Instantiate(Target target, IEnumerable<CaptionFilter> filters)
        {
            var builder = InstantiateBuilder(target);
            builder.Build(filters);

            var cmd = connection.CreateCommand();
            cmd.CommandText = builder.GetCommandText();

            var description = new CommandDescription(target, filters);

            var command = new RelationalCommand(cmd, description);

            return command;
        }


        protected virtual IDiscoveryCommandBuilder InstantiateBuilder(Target target)
        {
            switch (target)
            {
                case Target.Columns:
                    return new ColumnDiscoveryCommandBuilder();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
