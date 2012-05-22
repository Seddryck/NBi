using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core;
using NBi.Core.Query;
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
            var cmd = new SqlCommand(sql, new SqlConnection(ConnectionStringReader.GetSqlClient()));

            var mock = new Mock<IQueryPerformance>();
            mock.Setup(engine => engine.CheckPerformance(It.IsAny<IDbCommand>(),It.IsAny<bool>()))
                .Returns(new PerformanceResult(new System.TimeSpan(0,0,0,2)));
            IQueryPerformance qp = mock.Object;

            var fasterThanConstraint = new FasterThanConstraint(qp);
            fasterThanConstraint.MaxTimeMilliSeconds(5000);

            //Method under test
            Assert.That(cmd, fasterThanConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.CheckPerformance(It.IsAny<IDbCommand>(), It.IsAny<bool>()), Times.Once());
        }

    }
}
