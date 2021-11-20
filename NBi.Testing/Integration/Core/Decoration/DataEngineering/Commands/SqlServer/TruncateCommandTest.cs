using System;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceProcess;
using Moq;
using NBi.Core.Decoration.DataEngineering;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Decoration.DataEngineering.Commands.SqlServer
{
    [TestFixture]
    [Category("LocalSQL")]
    public class TruncateCommandTest
    {
        private const string SERVICE_NAME = "MSSQL$SQL2017";

        private int CountElementsInTable(string tableName, string connectionString)
        {
            int count = -1;
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandText = $"select count(*) from {tableName};"
                };
                conn.Open();
                count = (int)cmd.ExecuteScalar();
            }
            return count;
        }

        [SetUp]
        public void EnsureLocalSqlServerRunning()
        {
            var service = new ServiceController(SERVICE_NAME);
            if (service.Status != ServiceControllerStatus.Running)
                Assert.Ignore("Local SQL Server not started.");
        }

        public void CreateTemporaryTable(string tableName, string connectionString)
        {
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

        [Test]
        public void Execute_TruncateTemporaryTable_TableIsEmpty()
        {
            //Create table
            CreateTemporaryTable("Temporary", ConnectionStringReader.GetLocalSqlClient());

            //Mock the commandXml
            var resetArgs = Mock.Of<TableTruncateCommandArgs>(
                args => args.ConnectionString == ConnectionStringReader.GetLocalSqlClient()
                    && args.TableName == new LiteralScalarResolver<string>("Temporary")
                );

            var factory = new DataEngineeringFactory();
            var command = factory.Instantiate(resetArgs);

            command.Execute();

            //Execute Query on temporary table to knwo the new count of elements
            var after = CountElementsInTable("Temporary", ConnectionStringReader.GetLocalSqlClient());
            Assert.That(after, Is.EqualTo(0));
        }
    }
}
