using System;
using System.Linq;
using Moq;
using NBi.Core;
using NBi.NUnit.Execution;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Execution
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
        public void Matches_IsFasterThanMax_True()
        {
            var stub = new Mock<IExecution>();
            stub.Setup(e => e.Run())
                .Returns(Mock.Of<IExecutionResult>( r => r.TimeElapsed==new TimeSpan(0,0,0,0,100)));
            var engine = stub.Object;
            
            var fasterThanConstraint = new FasterThanConstraint();
            fasterThanConstraint = fasterThanConstraint.MaxTimeMilliSeconds(5000);

            Assert.That(fasterThanConstraint.Matches(engine), Is.True);
        }

        [Test]
        public void Matches_IsSlowerThanMax_False()
        {
            var stub = new Mock<IExecution>();
            stub.Setup(e => e.Run())
                .Returns(Mock.Of<IExecutionResult>(r => r.TimeElapsed == new TimeSpan(0, 0, 0, 100, 0)));
            var engine = stub.Object;

            var fasterThanConstraint = new FasterThanConstraint();
            fasterThanConstraint = fasterThanConstraint.MaxTimeMilliSeconds(5000);

            Assert.That(fasterThanConstraint.Matches(engine), Is.False);
        }
    }
}
