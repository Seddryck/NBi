using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core;
using NBi.Core.Query.Connection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Query.Connection
{
    [TestFixture]
    public class ConnectionFactoryTest
    {
        [Test]
        [TestCase("Provider=OleDb.1;Data Source=ds;Initial Catalog=ic;Integrated Security=SSPI;", typeof(OleDbConnection))]
        [TestCase("Data Source=ds;Initial Catalog=ic", typeof(SqlConnection))]
        [TestCase("Driver={SQL Server Native Client 10.0};Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;", typeof(OdbcConnection))]
        [TestCase("Provider = MSOLAP;Data Source = ds;Initial Catalog = ic", typeof(AdomdConnection))]
        public void Instantiate_ConnectionString_CorrectType(string connectionString, Type expectedType)
        {
            var factory = new ConnectionFactory();
            var connection = factory.Instantiate(connectionString);
            Assert.That(connection.CreateNew(), Is.TypeOf(expectedType));
        }

        #region Fake
        public class FakeConnection : IConnection
        {
            public string ConnectionString => "fake://MyConnectionString";

            public object CreateNew()
            {
                throw new NotImplementedException();
            }
        }

        public class FakeConnectionFactory : IConnectionFactory
        {
            public bool CanHandle(string connectionString)
            {
                return connectionString.StartsWith("fake://");
            }

            public IConnection Instantiate(string connectionString)
            {
                return new FakeConnection();
            }
        }

        #endregion

        [Test]
        public void Instantiate_AddCustom_CorrectType()
        {
            var factory = new ConnectionFactory();
            factory.AddFactory(new FakeConnectionFactory());
            var connection = factory.Instantiate("fake://MyConnectionString");
            Assert.IsInstanceOf<FakeConnection>(connection);
        }

        [Test]
        public void Add_TwiceTheSame_Exception()
        {
            var factory = new ConnectionFactory();
            factory.AddFactory(new FakeConnectionFactory());
            var ex = Assert.Throws<ArgumentException>(() => factory.AddFactory(new FakeConnectionFactory()));
            Assert.That(ex.Message.Contains(typeof(FakeConnectionFactory).Name));
        }
    }
}
