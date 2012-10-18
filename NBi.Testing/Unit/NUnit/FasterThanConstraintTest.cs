using System.Data.SqlClient;
using Moq;
using NBi.Core.Query;
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
        public void Matches_WithoutCleanCache_EngineCleanCacheNeverCalled()
        {
            var cmd = new SqlCommand();

            var mock = new Mock<IQueryPerformance>();
            mock.Setup(engine => engine.CheckPerformance())
                .Returns(new PerformanceResult(new System.TimeSpan(0,0,0,2)));
            mock.Setup(engine => engine.CleanCache());
            IQueryPerformance qp = mock.Object;

            var fasterThanConstraint = new NBi.NUnit.FasterThanConstraint() { Engine = qp };
            fasterThanConstraint = fasterThanConstraint.MaxTimeMilliSeconds(5000);

            //Method under test
            fasterThanConstraint.Matches(cmd);

            //Test conclusion            
            mock.Verify(engine => engine.CheckPerformance(), Times.Once());
            mock.Verify(engine => engine.CleanCache(), Times.Never());
        }

        [Test]
        public void Matches_IncludingCleanCache_EngineCleanCacheCalledOnce()
        {
            var cmd = new SqlCommand();

            var mock = new Mock<IQueryPerformance>();
            mock.Setup(engine => engine.CheckPerformance())
                .Returns(new PerformanceResult(new System.TimeSpan(0, 0, 0, 2)));
            mock.Setup(engine => engine.CleanCache());
            IQueryPerformance qp = mock.Object;

            var fasterThanConstraint = new NBi.NUnit.FasterThanConstraint() { Engine = qp };
            fasterThanConstraint = fasterThanConstraint.MaxTimeMilliSeconds(5000).CleanCache();

            //Method under test
            fasterThanConstraint.Matches(cmd);

            //Test conclusion            
            mock.Verify(engine => engine.CheckPerformance(), Times.Once());
            mock.Verify(engine => engine.CleanCache(), Times.Once());
        }

        [Test]
        public void Matches_ExecutionTooSlow_ReturnFalse()
        {
            var cmd = new SqlCommand();

            var stub = new Mock<IQueryPerformance>();
            stub.Setup(engine => engine.CheckPerformance())
                .Returns(new PerformanceResult(new System.TimeSpan(0, 0, 0, 8)));
            IQueryPerformance qp = stub.Object;

            var fasterThanConstraint = new NBi.NUnit.FasterThanConstraint() { Engine = qp };
            fasterThanConstraint.MaxTimeMilliSeconds(5000);

            //Method under test
            var res = fasterThanConstraint.Matches(cmd);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_ExecutionFastEnought_ReturnTRue()
        {
            var cmd = new SqlCommand();

            var stub = new Mock<IQueryPerformance>();
            stub.Setup(engine => engine.CheckPerformance())
                .Returns(new PerformanceResult(new System.TimeSpan(0, 0, 0, 4)));
            IQueryPerformance qp = stub.Object;

            var fasterThanConstraint = new NBi.NUnit.FasterThanConstraint() { Engine = qp };
            fasterThanConstraint.MaxTimeMilliSeconds(5000);

            //Method under test
            var res = fasterThanConstraint.Matches(cmd);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

    }
}
