using System.Data.SqlClient;
using Moq;
using NBi.Core.Query;
using NBi.NUnit.Query;
using NUnit.Framework;
using NBi.Core.Query.Performance;
using System;

namespace NBi.Testing.Unit.NUnit.Query
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
            var queryFoundry = new Mock<IQuery>();
            var query = queryFoundry.Object;

            var mock = new Mock<IPerformanceEngine>();
            mock.Setup(engine => engine.Execute(It.IsAny<TimeSpan>()))
                .Returns(new PerformanceResult(new TimeSpan(0,0,0,2)));
            mock.Setup(engine => engine.CleanCache());
            IPerformanceEngine qp = mock.Object;

            var fasterThanConstraint = new FasterThanConstraint() { Engine = qp };
            fasterThanConstraint = fasterThanConstraint.MaxTimeMilliSeconds(5000);

            //Method under test
            fasterThanConstraint.Matches(query);

            //Test conclusion            
            mock.Verify(engine => engine.Execute(It.IsAny<TimeSpan>()), Times.Once());
            mock.Verify(engine => engine.CleanCache(), Times.Never());
        }

        [Test]
        public void Matches_IncludingCleanCache_EngineCleanCacheCalledOnce()
        {
            var queryFoundry = new Mock<IQuery>();
            var query = queryFoundry.Object;

            var mock = new Mock<IPerformanceEngine>();
            mock.Setup(engine => engine.Execute(It.IsAny<TimeSpan>()))
                .Returns(new PerformanceResult(new TimeSpan(0, 0, 0, 2)));
            mock.Setup(engine => engine.CleanCache());
            IPerformanceEngine qp = mock.Object;

            var fasterThanConstraint = new FasterThanConstraint() { Engine = qp };
            fasterThanConstraint = fasterThanConstraint.MaxTimeMilliSeconds(5000).CleanCache();

            //Method under test
            fasterThanConstraint.Matches(query);

            //Test conclusion            
            mock.Verify(engine => engine.Execute(It.IsAny<TimeSpan>()), Times.Once());
            mock.Verify(engine => engine.CleanCache(), Times.Once());
        }

        [Test]
        public void Matches_ExecutionTooSlow_ReturnFalse()
        {
            var queryFoundry = new Mock<IQuery>();
            var query = queryFoundry.Object;

            var stub = new Mock<IPerformanceEngine>();
            stub.Setup(engine => engine.Execute(It.IsAny<TimeSpan>()))
                .Returns(new PerformanceResult(new TimeSpan(0, 0, 0, 8)));
            IPerformanceEngine qp = stub.Object;

            var fasterThanConstraint = new FasterThanConstraint() { Engine = qp };
            fasterThanConstraint.MaxTimeMilliSeconds(5000);

            //Method under test
            var res = fasterThanConstraint.Matches(query);

            //Test conclusion            
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_ExecutionFastEnought_ReturnTRue()
        {
            var queryFoundry = new Mock<IQuery>();
            var query = queryFoundry.Object;

            var stub = new Mock<IPerformanceEngine>();
            stub.Setup(engine => engine.Execute(It.IsAny<TimeSpan>()))
                .Returns(new PerformanceResult(new TimeSpan(0, 0, 0, 4)));
            IPerformanceEngine qp = stub.Object;

            var fasterThanConstraint = new FasterThanConstraint() { Engine = qp };
            fasterThanConstraint.MaxTimeMilliSeconds(5000);

            //Method under test
            var res = fasterThanConstraint.Matches(query);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

    }
}
