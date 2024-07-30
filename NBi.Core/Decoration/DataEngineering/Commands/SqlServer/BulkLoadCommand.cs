using NBi.Core.FlatFile;
using NBi.Extensibility;
using System;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;

namespace NBi.Core.Decoration.DataEngineering.Commands.SqlServer
{
    class BulkLoadCommand : IDecorationCommand
    {
        private readonly TableLoadCommandArgs args;

        public BulkLoadCommand(TableLoadCommandArgs args) => this.args = args;

        public void Execute()
        {
            Execute(args.ConnectionString, args.TableName.Execute() ?? string.Empty, args.FileName.Execute() ?? string.Empty);
        }

        internal void Execute(string connectionString, string tableName, string filename)
        {
            if (!File.Exists(filename))
                throw new ExternalDependencyNotFoundException(filename);

            using var connection = new SqlConnection(connectionString);
            // make sure to enable triggers
            // more on triggers in next post
            using var bulkCopy =
                new SqlBulkCopy
                (
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                )
                {
                    // set the destination table name
                    DestinationTableName = tableName
                };
            connection.Open();

            // write the data in the "dataTable"
            var fileReader = new CsvReader();
            var dataTable = fileReader.ToDataTable(filename, false);
            bulkCopy.WriteToServer(dataTable);

            connection.Close();

        }
    }
}
