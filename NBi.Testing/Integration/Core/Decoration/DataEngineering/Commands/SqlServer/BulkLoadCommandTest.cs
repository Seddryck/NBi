using System;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceProcess;
using Moq;
using NBi.Core.Decoration.DataEngineering;
using NBi.Core.Decoration.DataEngineering.Commands.SqlServer;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Decoration.DataEngineering.Commands.SqlServer
{
    [TestFixture]
    [Category("LocalSQL")]
    public class BulkLoadCommandTest
	{
        private const string SERVICE_NAME = "MSSQL$SQL2017";
        private const string FILE_CSV = "Load.csv";
        private string FileName { get; set;}

        
        public void CreateTemporaryTable(string tableName, string connectionString)
        {
            //Build the fullpath for the file to read
            FileName = DiskOnFile.CreatePhysicalFile(FILE_CSV, $"{string.Join(".", GetType().Namespace.Split('.').Reverse().Skip(2).Reverse().ToArray())}.{FILE_CSV}");

            //Create the table
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand() { Connection = conn };
                conn.Open();
                cmd.CommandText = $"if (exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo' and TABLE_NAME = '{tableName}')) begin drop table dbo.{tableName}; end";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"create table {tableName} (Id int IDENTITY(1,1), Name varchar (255));";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"insert into {tableName} values ('No name');";
                cmd.ExecuteNonQuery();
            }
        }

        [SetUp]
        public void EnsureLocalSqlServerRunning()
        {
            var service = new ServiceController(SERVICE_NAME);
            if (service.Status != ServiceControllerStatus.Running)
                Assert.Ignore("Local SQL Server not started.");
        }

        private int CountElementsInTable(string tableName, string connectionString)
        {
            int count = -1;
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = $"select count(*) from {tableName}"
                };
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
            var loadArgs = Mock.Of<ILoadCommandArgs>(
                args => args.ConnectionString== ConnectionStringReader.GetLocalSqlClient()
			        && args.TableName== new LiteralScalarResolver<string>("Temporary")
                    && args.FileName == new LiteralScalarResolver<string>(FileName)
                );

            var command = new BulkLoadCommand(loadArgs);

            command.Execute();

            //Execute Query on temporary table to knwo the new count of elements
            var after = CountElementsInTable("Temporary", ConnectionStringReader.GetLocalSqlClient());
            Assert.That(after, Is.GreaterThan(before));
		}
	}
}
