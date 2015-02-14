using System;
using System.Linq;
using Moq;
using NBi.Core.DataManipulation;
using NBi.Core.DataManipulation.SqlServer;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.DataManipulation
{
    [TestFixture]
    public class DataManipulationFactoryTest
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
        public void Get_IResetCommandAndSqlServer_TruncateCommand()
        {
            var command = Mock.Of<IResetCommand>(
                    m => m.ConnectionString== ConnectionStringReader.GetSqlClient() &&
                    m.TableName=="MyTableToTruncate"
                );
            
            var factory = new DataManipulationFactory();
            var impl = factory.Get(command);

            Assert.That(impl, Is.TypeOf<TruncateCommand>());
        }

        [Test]
        public void Get_ILoadCommandAndSqlServer_BulkLoadCommand()
        {
            var command = Mock.Of<ILoadCommand>(
                    m => m.ConnectionString == ConnectionStringReader.GetSqlClient() &&
                    m.TableName == "MyTableToTruncate" &&
                    m.FileName == "MyFileName.csv"
                );

            var factory = new DataManipulationFactory();
            var impl = factory.Get(command);

            Assert.That(impl, Is.TypeOf<BulkLoadCommand>());
        }
    }
}
