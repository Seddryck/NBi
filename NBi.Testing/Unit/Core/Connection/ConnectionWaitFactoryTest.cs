using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using NBi.Core.Connection;
using System.Data.SqlClient;
using NBi.Xml.Decoration.Command;

namespace NBi.Testing.Unit.Core.Connection
{
    [TestFixture]
    public class ConnectionWaitFactoryTest
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
        public void Get_IWaitConnectionCommand_ConnectionWaitCommand()
        {
            var command = Mock.Of<IConnectionWaitCommand>(
                    m => m.ConnectionString== ConnectionStringReader.GetSqlClient() &&
                    m.TimeOut==70000
                );

            var factory = new ConnectionWaitFactory();
            var impl = factory.Get(command);

            Assert.That(impl, Is.TypeOf<ConnectionWaitCommand>());
            var waitConnectionCommand = impl as ConnectionWaitCommand;
            Assert.That(waitConnectionCommand.Connection, Is.Not.Null);
            Assert.That(waitConnectionCommand.Connection, Is.TypeOf<SqlConnection>());
            Assert.That(waitConnectionCommand.TimeOut, Is.EqualTo(command.TimeOut));
        }

        [Test]
        public void Get_IWaitConnectionCommandWithDefault_ConnectionWaitCommand()
        {
            var command = new ConnectionWaitXml();
            command.SpecificConnectionString = ConnectionStringReader.GetSqlClient();
             
            var factory = new ConnectionWaitFactory();
            var impl = factory.Get(command);

            Assert.That(impl, Is.TypeOf<ConnectionWaitCommand>());
            var waitConnectionCommand = impl as ConnectionWaitCommand;
            Assert.That(waitConnectionCommand.TimeOut, Is.EqualTo(60000));
        }
    }
}
