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

            //Method under test
            Assert.That(sql, new FasterThanConstraint(_connectionString, 5000));

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
        public void FasterThanConstraint_NUnitAssertThat_EngineCalledOnce()
        {
            var sql = "SELECT * FROM Product;";
            var mock = new Mock<IQueryPerformance>();

            mock.Setup(engine => engine.Validate(It.IsAny<string>()))
                .Returns(Result.Success());
            IQueryPerformance qp = mock.Object;

            var fasterThanConstraint = new FasterThanConstraint(qp);

            //Method under test
            Assert.That(sql, fasterThanConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.Validate(sql), Times.Once());
        }

    }
}
