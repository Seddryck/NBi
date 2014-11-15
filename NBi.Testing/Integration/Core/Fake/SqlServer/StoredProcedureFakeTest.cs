using System;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceProcess;
using Moq;
using NBi.Core.Fake;
using NUnit.Framework;
using Microsoft.SqlServer.Management.Smo;

namespace NBi.Testing.Integration.Core.Fake.SqlServer
{
    [TestFixture]
    [Category("LocalSQL")]
    public class StoredProcedureFakeTest
	{
        private const string SERVICE_NAME = "MSSQL$SQL2014";
        private const string INITIAL_SCRIPT = "DELETE FROM [dbo].[DimCustomer] WHERE [CustomerAlternateKey]='TEST-NBI'; INSERT INTO [dbo].[DimCustomer] ([GeographyKey], [CustomerAlternateKey]) VALUES (1, 'TEST-NBI'); SELECT SCOPE_IDENTITY();";
        private string FileName { get; set;}


        public void CreateTemporaryStoredProcedure(string spName, string initialScript, string connectionString)
        {
            //Create the view
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandText = string.Format("if (exists (select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and SPECIFIC_NAME = '{0}')) begin drop procedure dbo.[{0}]; end", spName);
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("create procedure [dbo].[{0}] AS {1}", spName, initialScript);
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

        private int GetLastElement(string spName, string connectionString)
        {
            int count = -1;
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = spName;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                count = Convert.ToInt32(cmd.ExecuteScalar());
                trans.Rollback();
            }
            return count;
        }

        private int GetLastElement(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT MAX(CustomerKey) FROM [dbo].[DimCustomer];";
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        [Test]
        public void Initialize_CodeWithTopOne_ScriptIsCorrect()
        {
            //Create table
            CreateTemporaryStoredProcedure("TemporarySP", INITIAL_SCRIPT,ConnectionStringReader.GetLocalSqlClient());

            //Get the view object
            var server = new Server(ConnectionStringReader.GetLocalServer());
            var database = server.Databases[ConnectionStringReader.GetLocalDatabase()];
            var sp = database.StoredProcedures["TemporarySP"];

            //
            var spFake = new StoredProcedureFake(sp);
            spFake.Initialize();

            Assert.That(spFake.Script, Is.EqualTo(INITIAL_SCRIPT));
        }

        [Test]
		public void Fake_NoInsertion_MaxCustomerPKHasNotChanged()
		{
            //Create table
            CreateTemporaryStoredProcedure("TemporarySP", INITIAL_SCRIPT, ConnectionStringReader.GetLocalSqlClient());

            //Check how many elements are available in the table
            var before = GetLastElement(ConnectionStringReader.GetLocalSqlClient());

            //Get the sp object
            var server = new Server(ConnectionStringReader.GetLocalServer());
            var database = server.Databases[ConnectionStringReader.GetLocalDatabase()];
            var sp = database.StoredProcedures["TemporarySP"];

            //Fake the sp
            var spFake = new StoredProcedureFake(sp);
            spFake.Initialize();
            spFake.Fake("SELECT MAX(CustomerKey) FROM [dbo].[DimCustomer];");

            //Execute SP
            var after = GetLastElement("TemporarySP", ConnectionStringReader.GetLocalSqlClient());

            Assert.That(after, Is.EqualTo(before));
		}

        [Test]
        public void Rollback_FakeThen_MaxCustomerHasChanged()
        {
            //Create table
            CreateTemporaryStoredProcedure("TemporarySP", INITIAL_SCRIPT, ConnectionStringReader.GetLocalSqlClient());

            //Check how many elements are available in the table
            var before = GetLastElement(ConnectionStringReader.GetLocalSqlClient());

            //Get the view object
            var server = new Server(ConnectionStringReader.GetLocalServer());
            var database = server.Databases[ConnectionStringReader.GetLocalDatabase()];
            var sp = database.StoredProcedures["TemporarySP"];

            //Fake the SP then Rollback
            var spFake = new StoredProcedureFake(sp);
            spFake.Initialize();
            spFake.Fake("SELECT MAX(CustomerKey) FROM [dbo].[DimCustomer];");
            spFake.Rollback();

            //Execute Query on temporary table to knwo the new count of elements
            var after = GetLastElement("TemporarySP", ConnectionStringReader.GetLocalSqlClient());

            Assert.That(after, Is.GreaterThan(before));
        }

	}
}
