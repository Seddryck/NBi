#region Using directives
using System.Data.SqlClient;
using NBi.NUnit.Query;
using NUnit.Framework;
using Moq;
using NBi.Extensibility.Query;
#endregion

namespace NBi.Testing.Integration.NUnit.Query
{
    [TestFixture]
    public class FasterThanConstraintTest
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

        [Test, Category("Sql")]
        public void Matches_FasterThanMaxTime_Success()
        {
            var queryMock = new Mock<IQuery>();
            queryMock.SetupGet(x => x.ConnectionString).Returns(ConnectionStringReader.GetSqlClient());
            queryMock.SetupGet(x => x.Statement).Returns("WAITFOR DELAY '00:00:00';");

            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(1000);
            ctr = ctr.TimeOutMilliSeconds(2000);

            //Method under test
            Assert.That(queryMock.Object, ctr);
            queryMock.Verify(x => x.ConnectionString, Times.Once());
            queryMock.Verify(x => x.Statement, Times.Once());
        }

        [Test, Category("Sql")]
        public void Matches_SlowerThanMaxTime_Failure()
        {
            var queryMock = new Mock<IQuery>();
            queryMock.SetupGet(x => x.ConnectionString).Returns(ConnectionStringReader.GetSqlClient());
            queryMock.SetupGet(x => x.Statement).Returns("WAITFOR DELAY '00:00:01';");

            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(100);
            ctr = ctr.TimeOutMilliSeconds(5000);

            Assert.That(ctr.Matches(queryMock.Object), Is.False);
        }

        [Test, Category("Sql")]
        public void Matches_SlowerThanMaxTimeAndTimeOut_Failure()
        {
            var queryMock = new Mock<IQuery>();
            queryMock.SetupGet(x => x.ConnectionString).Returns(ConnectionStringReader.GetSqlClient());
            queryMock.SetupGet(x => x.Statement).Returns("WAITFOR DELAY '00:00:10';");

            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(100);
            ctr = ctr.TimeOutMilliSeconds(1000);

            Assert.That(ctr.Matches(queryMock.Object), Is.False);
        }
    }

}
