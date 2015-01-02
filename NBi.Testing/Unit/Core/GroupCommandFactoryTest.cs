using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using NBi.Core.DataManipulation;
using NBi.Core.DataManipulation.SqlServer;
using NUnit.Framework;
using NBi.Core;

namespace NBi.Testing.Unit.Core
{
    [TestFixture]
    public class GroupCommandFactoryTest
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
        public void Get_IGroupCommand_GroupCommand()
        {
            var command = Mock.Of<IResetCommand>(
                    m => m.ConnectionString == ConnectionStringReader.GetSqlClient() &&
                    m.TableName == "MyTableToTruncate"
                );

            var otherCommand = Mock.Of<IResetCommand>(
                    m => m.ConnectionString == ConnectionStringReader.GetSqlClient() &&
                    m.TableName == "MyOtherTableToTruncate"
                );
            var commands = new List<IDecorationCommand>();
            commands.Add(command);
            commands.Add(otherCommand);

            var group = Mock.Of<IGroupCommand>(
                    g => g.Parallel==true &&
                    g.Commands == commands
                );
            
            var factory = new GroupCommandFactory();
            var impl = factory.Get(group);

            Assert.That(impl, Is.TypeOf<GroupCommand>());
        }

    }
}
