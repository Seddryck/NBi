using System;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.DataManipulation.SqlServer
{
    class BulkLoadCommand : IDataManipulationImplementation
	{
		private readonly string connectionString;
		private readonly string tableName;
		private readonly string fileName;

		public BulkLoadCommand(ILoadCommand command)
		{
			connectionString = command.ConnectionString;
			tableName = command.TableName;
			fileName = command.FileName;
		}

		public void Execute()
		{
			Execute(connectionString, tableName, fileName);
		}

		internal void Execute(string connectionString,string tableName, string filename)
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
					);

				// set the destination table name
				bulkCopy.DestinationTableName = tableName;
				connection.Open();

				// write the data in the "dataTable"
                var fileReader = new CsvReader(filename, false);
				var dataTable = fileReader.Read();
				bulkCopy.WriteToServer(dataTable);

				connection.Close();
			}

}
	}
}
