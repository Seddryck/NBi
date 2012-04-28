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
    public class SyntacticallyCorrectConstraintTest
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
        public void QueryParserRealImplementation_SyntacticallyCorrectConstraint_Success()
        {
            var sql = "SELECT * FROM Product;";
            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);


            //Method under test
            Assert.That(cmd, new SyntacticallyCorrectConstraint());

            //Test conclusion            
            Assert.Pass();
        }

        [Test]
        public void QueryParserRealImplementation_IsSyntacticallyCorrect_Success()
        {
            var sql = "SELECT * FROM Product;";
            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            Assert.That(cmd, NBi.NUnit.Is.SyntacticallyCorrect());
            
            Assert.Pass();
        }

        [Test]
        public void SyntacticallyCorrectConstraint_NUnitAssertThatIDbCommand_EngineCalledOnce()
        {

            var mock = new Mock<IQueryParser>();
            mock.Setup(engine => engine.Validate(It.IsAny<IDbCommand>()))
                .Returns(Result.Success());
            IQueryParser qp = mock.Object;

            var syntacticallyCorrectConstraint = new SyntacticallyCorrectConstraint(qp);

            //Method under test
            Assert.That(new SqlCommand(), syntacticallyCorrectConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.Validate(It.IsAny<IDbCommand>()), Times.Once());
        }

    }
}
