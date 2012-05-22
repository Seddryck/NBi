using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core;
using NBi.Core.Query;
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
            mock.Setup(engine => engine.Parse(It.IsAny<IDbCommand>()))
                .Returns(ParserResult.NoParsingError());
            IQueryParser qp = mock.Object;

            var syntacticallyCorrectConstraint = new SyntacticallyCorrectConstraint(qp);

            //Method under test
            Assert.That(new SqlCommand(), syntacticallyCorrectConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.Parse(It.IsAny<IDbCommand>()), Times.Once());
        }

    }
}
