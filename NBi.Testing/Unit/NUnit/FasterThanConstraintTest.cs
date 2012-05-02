using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core;
using NBi.Core.Database;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit
{
    [TestFixture]
    public class FasterThanConstraintTest
    {

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
           
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        

        [Test]
        public void FasterThanConstraint_NUnitAssertThatIDbCommand_EngineCalledOnce()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.Get()));

            var mock = new Mock<IQueryPerformance>();
            mock.Setup(engine => engine.Validate(It.IsAny<IDbCommand>()))
                .Returns(Result.Success());
            IQueryPerformance qp = mock.Object;

            var fasterThanConstraint = new FasterThanConstraint(qp);

            //Method under test
            Assert.That(cmd, fasterThanConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.Validate(It.IsAny<IDbCommand>()), Times.Once());
        }

    }
}
