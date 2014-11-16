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
    public class ViewFakeTest
	{
        private string ServiceName
        {
            get { return ConnectionStringReader.GetSqlServerServiceName(); }
        }
        private const string INITIAL_SCRIPT = "SELECT top 100 [FirstName], [LastName] FROM [dbo].[DimCustomer]";
        private string FileName { get; set;}


        public void CreateTemporaryView(string viewName, string initialScript, string connectionString)
        {
            //Create the view
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandText = string.Format("if (exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo' and TABLE_NAME = '{0}')) begin drop view dbo.[{0}]; end", viewName);
                cmd.ExecuteNonQuery();
                cmd.CommandText = string.Format("CREATE VIEW [dbo].[{0}] AS {1}", viewName, initialScript);
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

        private int CountElementsInView(string viewName, string connectionString)
        {
            int count = -1;
            using (var conn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("select count(*) from dbo.[{0}];", viewName);
                conn.Open();
                count = (int) cmd.ExecuteScalar();
            }
            return count;
        }

        [Test]
        public void Initialize_CodeWithTopOne_ScriptIsCorrect()
        {
            //Create table
            CreateTemporaryView("TemporaryVW", INITIAL_SCRIPT,ConnectionStringReader.GetLocalSqlClient());

            //Get the view object
            var server = new Server(ConnectionStringReader.GetLocalServer());
            var database = server.Databases[ConnectionStringReader.GetLocalDatabase()];
            var view = database.Views["TemporaryVW"];

            //
            var viewFake = new ViewFake(view);
            viewFake.Initialize();

            Assert.That(viewFake.Script, Is.EqualTo(INITIAL_SCRIPT));
        }

        [Test]
		public void Fake_CodeWithTopOne_FakeIsActiveAndReturnLessRows()
		{
            //Create table
            CreateTemporaryView("TemporaryVW", INITIAL_SCRIPT, ConnectionStringReader.GetLocalSqlClient());

            //Check how many elements are available in the table
            var before = CountElementsInView("TemporaryVW", ConnectionStringReader.GetLocalSqlClient());

            //Get the view object
            var server = new Server(ConnectionStringReader.GetLocalServer());
            var database = server.Databases[ConnectionStringReader.GetLocalDatabase()];
            var view = database.Views["TemporaryVW"];

            //
            var viewFake = new ViewFake(view);
            viewFake.Initialize();
            viewFake.Fake("SELECT top 1 [FirstName], [LastName] FROM [dbo].[DimCustomer];");

            //Execute Query on temporary table to knwo the new count of elements
            var after = CountElementsInView("TemporaryVW", ConnectionStringReader.GetLocalSqlClient());

            Assert.That(after, Is.Not.EqualTo(before));
		}

        [Test]
        public void Rollback_FakeThen_FakeIsActiveAndReturnLessRows()
        {
            //Create table
            CreateTemporaryView("TemporaryVW", INITIAL_SCRIPT, ConnectionStringReader.GetLocalSqlClient());

            //Check how many elements are available in the table
            var before = CountElementsInView("TemporaryVW", ConnectionStringReader.GetLocalSqlClient());

            //Get the view object
            var server = new Server(ConnectionStringReader.GetLocalServer());
            var database = server.Databases[ConnectionStringReader.GetLocalDatabase()];
            var view = database.Views["TemporaryVW"];

            //
            var viewFake = new ViewFake(view);
            viewFake.Initialize();
            viewFake.Fake("SELECT top 1 [FirstName], [LastName] FROM [dbo].[DimCustomer];");
            viewFake.Rollback();

            //Execute Query on temporary table to knwo the new count of elements
            var after = CountElementsInView("TemporaryVW", ConnectionStringReader.GetLocalSqlClient());

            Assert.That(after, Is.EqualTo(before));
        }

	}
}
