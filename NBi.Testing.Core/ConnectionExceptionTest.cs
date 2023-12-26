﻿using System;
using System.Data.OleDb;
using System.Linq;
using NBi.Core;
using NBi.Extensibility;
using NUnit.Framework;

namespace NBi.Core.Testing
{
    [TestFixture]
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
                    Assert.That(nbiEx.Message, Does.Contain(connectionString));
                }
            }
        }
    }
}
