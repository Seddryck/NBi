using System;
using System.Linq;
using Moq;
using NBi.Core;
using NBi.NUnit.Execution;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit.Execution
{
    [TestFixture]
    public class SuccessfulConstraintTest
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
        public void Matches_IsSuccessful_True()
        {
            var stub = new Mock<IExecution>();
            stub.Setup(e => e.Run())
                .Returns(Mock.Of<IExecutionResult>(r => r.IsSuccess == true));
            var engine = stub.Object;

            var successfulConstraint = new SuccessfulConstraint();

            Assert.That(successfulConstraint.Matches(engine), Is.True);
        }

        [Test]
        public void Matches_IsFailure_False()
        {
            var stub = new Mock<IExecution>();
            stub.Setup(e => e.Run())
                .Returns(Mock.Of<IExecutionResult>(r => r.IsSuccess == false));
            var engine = stub.Object;

            var successfulConstraint = new SuccessfulConstraint();

            Assert.That(successfulConstraint.Matches(engine), Is.False);
        }
    }
}
