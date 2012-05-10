using System.Collections;
using Moq;
using NBi.Core;
using NBi.Core.Analysis.Query;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit
{
    public class CountConstraintTest
    {
        [Test]
        public void CountConstraint_NUnitAssertThatICollection_EngineCalledOnce()
        {
            var mock = new Mock<ICollectionEngine>();
            mock.Setup(engine => engine.Validate(It.IsAny<ICollection>()))
                .Returns(Result.Success());
            ICollectionEngine collEngine = mock.Object;

            var countConstraint = new CountConstraint(collEngine).Exactly(2);
            var coll = new ArrayList();

            //Method under test
            Assert.That(coll, countConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.Validate(coll), Times.Once());
        }

        [Test]
        public void Exactly_PositiveValue_ValueRegisteredInEngine()
        {
            var mock = new Mock<ICollectionEngine>();
            ICollectionEngine collEngine = mock.Object;

            var countConstraint = new CountConstraint(collEngine);
            
            //Method under test
            countConstraint.Exactly(2);

            //Test conclusion            
            mock.VerifySet(engine => engine.Exactly=2);
        }

        [Test]
        public void MoreThan_PositiveValue_ValueRegisteredInEngine()
        {
            var mock = new Mock<ICollectionEngine>();
            ICollectionEngine collEngine = mock.Object;

            var countConstraint = new CountConstraint(collEngine);

            //Method under test
            countConstraint.MoreThan(2);

            //Test conclusion            
            mock.VerifySet(engine => engine.MoreThan = 2);
        }

        [Test]
        public void LessThan_PositiveValue_ValueRegisteredInEngine()
        {
            var mock = new Mock<ICollectionEngine>();
            ICollectionEngine collEngine = mock.Object;

            var countConstraint = new CountConstraint(collEngine);

            //Method under test
            countConstraint.LessThan(2);

            //Test conclusion            
            mock.VerifySet(engine => engine.LessThan = 2);
        }
    }
}
