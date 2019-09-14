using NBi.Core.FlatFile;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.Decoration.DataEngineering.Commands.SqlServer
{
    class BulkLoadCommand : IDecorationCommand
    {
        private readonly ILoadCommandArgs args;

        public BulkLoadCommand(ILoadCommandArgs args) => this.args = args;

        public void Execute()
        {
            Execute(args.ConnectionString, args.TableName.Execute(), args.FileName.Execute());
        }

        internal void Execute(string connectionString, string tableName, string filename)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                // make sure to enable triggers
                // more on triggers in next post
                using (var bulkCopy =
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
                    }
                )
                {
                    connection.Open();

                    // write the data in the "dataTable"
                    var fileReader = new CsvReader();
                    var dataTable = fileReader.ToDataTable(filename, false);
                    bulkCopy.WriteToServer(dataTable);

                    connection.Close();
                }
            }

        }
    }
}
