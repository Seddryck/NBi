using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core;
using NBi.Core.Database;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit
{
    [TestFixture]
    public class SyntacticallyCorrectConstraintTest
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
