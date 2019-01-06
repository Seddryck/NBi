using NBi.Core.FlatFile;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.DataManipulation.SqlServer
{
    class BulkLoadCommand : IDecorationCommandImplementation
    {
        private readonly string connectionString;
        private readonly string tableName;
        private readonly string fileName;

        public BulkLoadCommand(ILoadCommand command, SqlConnection connection)
        {
            connectionString = connection.ConnectionString;
            tableName = command.TableName;
            fileName = command.FileName;
        }

        public void Execute()
        {
            Execute(connectionString, tableName, fileName);
        }

        internal void Execute(string connectionString, string tableName, string filename)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                // make sure to enable triggers
                // more on triggers in next post
                SqlBulkCopy bulkCopy =
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
}
