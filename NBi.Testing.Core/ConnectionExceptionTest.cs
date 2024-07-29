using System;
using System.Data.OleDb;
using System.Linq;
using NBi.Core;
using NBi.Extensibility;
using NUnit.Framework;

namespace NBi.Core.Testing
{
    [TestFixture]
    [Platform("Win")]
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class ConnectionExceptionTest
    {
        
        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void Ctor_ExceptionCreated_ContainsConnectionString()
        {
            // Open the connection
            using var connection = new OleDbConnection();
            var connectionString = "CONNECTION STRING TO DISPLAY";
            var ex = Assert.Catch<Exception>(() => connection.ConnectionString = connectionString);
            var nbiEx = Assert.Catch<ConnectionException>(() => throw new ConnectionException(ex!, connectionString));
            Assert.That(nbiEx!.Message, Does.Contain(connectionString));
        }
    }
}
