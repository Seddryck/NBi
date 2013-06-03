using System;
using System.Data.OleDb;
using System.Linq;
using NBi.Core;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core
{
    [TestFixture]
    public class ConnectionExceptionTest
    {
        
        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
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
            NBiException nbiEx = null;
            // Open the connection
            using (var connection = new OleDbConnection())
            {
                var connectionString = "CONNECTION STRING TO DISPLAY"; 
                try
                { connection.ConnectionString = connectionString; }
                catch (ArgumentException ex)
                {
                    nbiEx = Assert.Catch<NBiException>(delegate { throw new ConnectionException(ex, connectionString); });
                }
                if (nbiEx == null)
                    Assert.Fail("An exception should have been thrown");
                else
                {
                    //Test can continue
                    Console.Out.WriteLine(nbiEx.Message);
                    Assert.That(nbiEx.Message, Is.StringContaining(connectionString));
                }
            }
        }
    }
}
