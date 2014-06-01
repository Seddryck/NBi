using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.DataManipulation.SqlServer
{
	class TruncateCommand : IDecorationCommandImplementation
	{
		private readonly string connectionString;
		private readonly string tableName;

		public TruncateCommand(IResetCommand command)
		{
			connectionString = command.ConnectionString;
			tableName = command.TableName;
		}

		public void Execute()
		{
			Execute(connectionString, tableName);
		}

		internal void Execute(string connectionString, string tableName)
		{           
			using (var conn = new SqlConnection(connectionString))
			{
				var cmd = new SqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = string.Format ("truncate table {0};", tableName);
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			}
		}

	}
}
