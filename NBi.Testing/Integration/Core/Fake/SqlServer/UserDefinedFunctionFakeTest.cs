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
    public class UserDefinedFunctionFakeTest
	{
        private string ServiceName
        {
            get { return ConnectionStringReader.GetSqlServerServiceName(); }
        }
        private const string INITIAL_SCRIPT = "BEGIN RETURN cast(convert(varchar, @year) + '-' + [dbo].[udfTwoDigitZeroFill](@month) + '-' + [dbo].[udfTwoDigitZeroFill](@day) + 'T00:00:00' as varchar(20)); END";

        public void CreateTemporaryUdf(string udfName, string initialScript, string connectionString)
        {
            //Create the view
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandText = string.Format("if (exists (select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'function' and SPECIFIC_NAME = '{0}')) begin drop function dbo.[{0}]; end", udfName);
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("CREATE FUNCTION [dbo].{0} (@year int, @month int, @day int) RETURNS varchar(20) AS {1}", udfName, initialScript);
                cmd.ExecuteNonQuery();
            }
        }

        [SetUp]
        public void EnsureLocalSqlServerRunning()
        {
            var service = new ServiceController(ServiceName);
            if (service.Status != ServiceControllerStatus.Running)
                Assert.Ignore("Local SQL Server not started.");
        }

        private string ApplyUdf(string udfName, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("select dbo.[{0}](1978,12,28);", udfName);
                conn.Open();
                return (string)cmd.ExecuteScalar();
            }
        }

        [Test]
        public void Initialize_CodeWithStandardReturn_ScriptIsCorrect()
        {
            //Create table
            CreateTemporaryUdf("TemporaryUdf", INITIAL_SCRIPT,ConnectionStringReader.GetLocalSqlClient());

            //Get the view object
            var server = new Server(ConnectionStringReader.GetLocalServer());
            var database = server.Databases[ConnectionStringReader.GetLocalDatabase()];
            var udf = database.UserDefinedFunctions["TemporaryUdf"];

            //
            var udfFake = new UserDefinedFunctionFake(udf);
            udfFake.Initialize();

            Assert.That(udfFake.Script, Is.EqualTo(INITIAL_SCRIPT));
        }

        [Test]
		public void Fake_AlternateResult_FakeIsActiveAndReturnAlternateResult()
		{
            //Create table
            CreateTemporaryUdf("TemporaryUdf", INITIAL_SCRIPT, ConnectionStringReader.GetLocalSqlClient());

            //Check how many elements are available in the table
            var before = ApplyUdf("TemporaryUdf", ConnectionStringReader.GetLocalSqlClient());

            //Get the view object
            var server = new Server(ConnectionStringReader.GetLocalServer());
            var database = server.Databases[ConnectionStringReader.GetLocalDatabase()];
            var udf = database.UserDefinedFunctions["TemporaryUdf"];

            //
            var udfFake = new UserDefinedFunctionFake(udf);
            udfFake.Initialize();
            udfFake.Fake("BEGIN RETURN cast([dbo].[udfTwoDigitZeroFill](@day) + '/' + [dbo].[udfTwoDigitZeroFill](@month) + '/' + convert(varchar,@year) as varchar(20)); END");

            //Execute Query on temporary table to knwo the new count of elements
            var after = ApplyUdf("TemporaryUdf", ConnectionStringReader.GetLocalSqlClient());

            Assert.That(after, Is.Not.EqualTo(before));
            Assert.That(after, Is.EqualTo("28/12/1978"));
		}

        [Test]
        public void Rollback_FakeSupplied_FakeIsNotActiveAymore()
        {
            //Create table
            CreateTemporaryUdf("TemporaryUdf", INITIAL_SCRIPT, ConnectionStringReader.GetLocalSqlClient());

            //Check how many elements are available in the table
            var before = ApplyUdf("TemporaryUdf", ConnectionStringReader.GetLocalSqlClient());

            //Get the view object
            var server = new Server(ConnectionStringReader.GetLocalServer());
            var database = server.Databases[ConnectionStringReader.GetLocalDatabase()];
            var udf = database.UserDefinedFunctions["TemporaryUdf"];

            //
            var udfFake = new UserDefinedFunctionFake(udf);
            udfFake.Initialize();
            udfFake.Fake("BEGIN RETURN cast([dbo].[udfTwoDigitZeroFill](@day) + '/' + [dbo].[udfTwoDigitZeroFill](@month) + '/' + convert(varchar,@year) as varchar(20)); END");
            udfFake.Rollback();

            //Execute Query on temporary table to knwo the new count of elements
            var after = ApplyUdf("TemporaryUdf", ConnectionStringReader.GetLocalSqlClient());

            Assert.That(after, Is.EqualTo(before));
            Assert.That(after, Is.EqualTo("1978-12-28T00:00:00"));
        }

	}
}
