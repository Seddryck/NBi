#region Using directives
using System.Data;
using System.Data.OleDb;
using Moq;
using NBi.Core.ResultSet;
using NUnit.Framework;

#endregion

namespace NBi.Testing.Unit.Core.Analysis.Query
{
    [TestFixture]
    public class ResultSetComparerTest
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
        public void Validate_ctorWithIDbCommand_ExecuteMethodGetExpectedResultSetWithCommand()
        {
            var mock = new Mock<ResultSetComparer>(new OleDbCommand(), string.Empty, string.Empty);
            mock.Setup(engine => engine.GetActualResultSet(It.IsAny<IDbCommand>()))
                .Returns(string.Empty);
            mock.Setup(engine => engine.GetExpectedResultSetWithCommand())
                .Returns(string.Empty);

            IResultSetComparer rsc = mock.Object;

            //Method under test
            rsc.Validate(new OleDbCommand());

            //Test conclusion            
            mock.Verify(engine => engine.GetExpectedResultSetWithCommand(), Times.Once());
            mock.Verify(engine => engine.GetExpectedResultSetWithFile(), Times.Never());
        }

        [Test]
        public void Validate_ctorWithResultSet_ExecuteMethodGetExpectedResultSetWithFile()
        {
            var mock = new Mock<ResultSetComparer>(string.Empty);
            mock.Setup(engine => engine.GetActualResultSet(It.IsAny<IDbCommand>()))
                .Returns(string.Empty);
            mock.Setup(engine => engine.GetExpectedResultSetWithFile())
                .Returns(string.Empty);

            IResultSetComparer rsc = mock.Object;

            //Method under test
            rsc.Validate(new OleDbCommand());

            //Test conclusion            
            mock.Verify(engine => engine.GetExpectedResultSetWithCommand(), Times.Never());
            mock.Verify(engine => engine.GetExpectedResultSetWithFile(), Times.Once());
        }

        [Test]
        public void Validate_TwoIdenticalStringsRetrievedForActualAndExpected_ReturnSuccess()
        {
            var mock = new Mock<ResultSetComparer>(string.Empty);
            mock.Setup(engine => engine.GetActualResultSet(It.IsAny<IDbCommand>()))
                .Returns("a;b;c");
            mock.Setup(engine => engine.GetExpectedResultSetWithFile())
                .Returns("a;b;c");

            IResultSetComparer rsc = mock.Object;

            //Method under test and Test conclusion
            Assert.That(rsc.Validate(new OleDbCommand()).Value, Is.EqualTo(NBi.Core.Result.Success().Value));
        }

        [Test]
        public void Validate_TwoDifferentStringsRetrievedForActualAndExpected_ReturnFailed()
        {
            var mock = new Mock<ResultSetComparer>(string.Empty);
            mock.Setup(engine => engine.GetActualResultSet(It.IsAny<IDbCommand>()))
                .Returns("a;b;c");
            mock.Setup(engine => engine.GetExpectedResultSetWithFile())
                .Returns("x;y");

            IResultSetComparer rsc = mock.Object;

            var res = rsc.Validate(new OleDbCommand());

            //Method under test and Test conclusion
            Assert.That(res.Value, Is.EqualTo(NBi.Core.Result.Failed().Value));
        }

    }
}
