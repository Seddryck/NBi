using NBi.Core.Decoration.DataEngineering;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.Decoration.DataEngineering.Commands.SqlServer
{
    class TruncateCommand : IDecorationCommand
    {
        private readonly IResetCommandArgs args;

        public TruncateCommand(IResetCommandArgs args) => this.args = args;

        public void Execute() => Execute(args.ConnectionString, args.TableName.Execute());

        internal void Execute(string connectionString, string tableName)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = $"truncate table {tableName};"
                };
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
