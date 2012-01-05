using Moq;
using NBi.Core;
using NBi.Core.Database;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.NUnit.Database
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

            //Method under test
            Assert.That(sql, new SyntacticallyCorrectConstraint(_connectionString));

            //Test conclusion            
            Assert.Pass();
        }

        [Test]
        public void QueryParserRealImplementation_IsSyntacticallyCorrect_Success()
        {
            var sql = "SELECT * FROM Product;";

            Assert.That(sql, OnDataSource.Localized(_connectionString).Is.SyntacticallyCorrect());
            
            Assert.Pass();
        }

        [Test]
        public void QueryParserMock_IsSyntacticallyCorrect_CalledOnce()
        {
            var sql = "SELECT * FROM Product;";
            var mock = new Mock<IQueryParser>();

            mock.Setup(engine => engine.ValidateFormat(sql))
                .Returns(Result.Success());
            IQueryParser qp = mock.Object;

            var qpc = new SyntacticallyCorrectConstraint(qp);

            //Method under test
            Assert.That(sql, qpc);

            //Test conclusion            
            mock.Verify(engine => engine.ValidateFormat(sql), Times.AtMostOnce());
        }

    }
}
