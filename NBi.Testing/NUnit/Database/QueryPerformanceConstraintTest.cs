using Moq;
using NBi.Core;
using NBi.Core.Database;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.NUnit.Database
{
    [TestFixture]
    public class QueryPerformanceConstraintTest
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
        public void QueryPerformanceRealImplementation_QueryPerformanceConstraint_Success()
        {
            var sql = "SELECT * FROM Product;";

            //Method under test
            Assert.That(sql, new QueryPerformanceConstraint(_connectionString, 5000));

            //Test conclusion            
            Assert.Pass();
        }

        [Test]
        public void QueryPerformanceRealImplementation_IsFasterThan_Success()
        {
            var sql = "SELECT * FROM Product;";

            Assert.That(sql, OnDataSource.Localized(_connectionString).Is.FasterThan(5000));
            
            Assert.Pass();
        }

        [Test]
        public void QueryPerformanceMock_IsFasterThan_CalledOnce()
        {
            var sql = "SELECT * FROM Product;";
            var mock = new Mock<IQueryPerformance>();

            mock.Setup(engine => engine.Validate(sql))
                .Returns(Result.Success())
                .AtMostOnce();
            IQueryPerformance qp = mock.Object;

            var qpc = new QueryPerformanceConstraint(qp);

            //Method under test
            Assert.That(sql, qpc);

            //Test conclusion            
            mock.Verify(engine => engine.Validate(sql));
        }

    }
}
