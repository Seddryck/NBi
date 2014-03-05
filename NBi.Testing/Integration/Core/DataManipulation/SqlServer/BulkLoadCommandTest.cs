using System;
using System.Data.SqlClient;
using System.Linq;
using Moq;
using NBi.Core.DataManipulation;
using NBi.Core.DataManipulation.SqlServer;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.DataManipulation.SqlServer
{
    [TestFixture]
    [Category("Local SQL instance")]
    public class BulkLoadCommandTest
	{
		private const string FILE_CSV = "Load.csv";
        private string FileName { get; set;}

        
        public void CreateTemporaryTable(string tableName, string connectionString)
        {
            //Build the fullpath for the file to read
            FileName = DiskOnFile.CreatePhysicalFile(FILE_CSV, "NBi.Testing.Integration.Core.DataManipulation." + FILE_CSV);

            //Create the table
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandText = string.Format("if (exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo' and TABLE_NAME = '{0}')) begin drop table dbo.{0}; end", tableName);
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("create table {0} (Id int IDENTITY(1,1), Name varchar (255));", tableName);
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("insert into {0} values ('No name');", tableName);
                cmd.ExecuteNonQuery();
            }
        }

        private int CountElementsInTable(string tableName, string connectionString)
        {
            int count = -1;
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("select count(*) from {0};", tableName);
                conn.Open();
                count = (int) cmd.ExecuteScalar();
            }
            return count;
        }

        [Test]
		public void Execute_LoadTemporaryTableBasedOnCSV_TableHasMoreData()
		{
            //Create table
            CreateTemporaryTable("Temporary", ConnectionStringReader.GetLocalSqlClient());

            //Check how many elements are available in the table
            var before = CountElementsInTable("Temporary", ConnectionStringReader.GetLocalSqlClient());

             //Mock the commandXml
            var info = Mock.Of<ILoadCommand>(
                command => command.ConnectionString== ConnectionStringReader.GetLocalSqlClient()
			        && command.TableName=="Temporary"
                    && command.FileName == FileName
                );

            //Apply the test
			var cmd = new BulkLoadCommand(info);
            cmd.Execute();

            //Execute Query on temporary table to knwo the new count of elements
            var after = CountElementsInTable("Temporary", ConnectionStringReader.GetLocalSqlClient());

            Assert.That(after, Is.GreaterThan(before));
		}

	}
}
