using Moq;
using NBi.Core;
using NBi.Core.Database;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.NUnit.Database
{
    [TestFixture]
    public class QueryParserConstraintTest
    {

        protected string _connectionString;

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
            _connectionString = @"Data Source=.\SqlExpress;Initial Catalog=NBi.Testing;Integrated Security=True";
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void QueryParserRealImplementation_QueryParserConstraint_Success()
        {
            var sql = "SELECT * FROM Product;";

            //Method under test
            Assert.That(sql, new QueryParserConstraint(_connectionString));

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
                .Returns(Result.Success())
                .AtMostOnce();
            IQueryParser qp = mock.Object;

            var qpc = new QueryParserConstraint(qp);

            //Method under test
            Assert.That(sql, qpc);

            //Test conclusion            
            mock.Verify(engine => engine.ValidateFormat(sql));
        }

    }
}
