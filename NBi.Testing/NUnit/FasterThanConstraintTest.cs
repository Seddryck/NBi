using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core;
using NBi.Core.Database;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.NUnit
{
    [TestFixture]
    public class FasterThanConstraintTest
    {

        protected string _connectionString;

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
            //If available use the user file
            if (System.IO.File.Exists("ConnectionString.user.config"))
            {
                _connectionString = System.IO.File.ReadAllText("ConnectionString.user.config");
            }
            else if (System.IO.File.Exists("ConnectionString.config"))
            {
                _connectionString = System.IO.File.ReadAllText("ConnectionString.config");
            }
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void QueryPerformanceRealImplementation_FasterThanConstraint_Success()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(_connectionString));

            //Method under test
            Assert.That(cmd, new FasterThanConstraint(5000, true));

            //Test conclusion            
            Assert.Pass();
        }

        [Test]
        public void QueryPerformanceRealImplementation_IsFasterThan_Success()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(_connectionString));

            //Method under test
            Assert.That(cmd, NBi.NUnit.Is.FasterThan(5000, true));
            
            Assert.Pass();
        }

        [Test]
        public void FasterThanConstraint_NUnitAssertThatIDbCommand_EngineCalledOnce()
        {
            var sql = "SELECT * FROM Product;";
            var cmd = new SqlCommand(sql, new SqlConnection(_connectionString));

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
