using System.Data.SqlClient;
using Moq;
using NBi.Core.Query;
using NBi.NUnit.Query;
using NUnit.Framework;
using NBi.Core.Query.Validation;

namespace NBi.Testing.Unit.NUnit.Query
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
        public void Matches_AnyIDbCommand_EngineParseCalledOnce()
        {

            var mock = new Mock<IValidationEngine>();
            mock.Setup(engine => engine.Parse())
                .Returns(ParserResult.NoParsingError());
            IValidationEngine qp = mock.Object;

            var syntacticallyCorrectConstraint = new SyntacticallyCorrectConstraint() { Engine = qp };

            var queryFoundry = new Mock<IQuery>();
            var query = queryFoundry.Object;

            syntacticallyCorrectConstraint.Matches(query);

            //Test conclusion            
            mock.Verify(engine => engine.Parse(), Times.Once());
        }

        [Test]
        public void Matches_NoParsingError_ReturnTrue()
        {

            var mock = new Mock<IValidationEngine>();
            mock.Setup(engine => engine.Parse())
                .Returns(ParserResult.NoParsingError());
            IValidationEngine qp = mock.Object;

            var syntacticallyCorrectConstraint = new SyntacticallyCorrectConstraint() { Engine = qp };

            var queryFoundry = new Mock<IQuery>();
            var query = queryFoundry.Object;

            //Method under test
            var res = syntacticallyCorrectConstraint.Matches(query);

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_WithParsingError_ReturnFalse()
        {

            var mock = new Mock<IValidationEngine>();
            mock.Setup(engine => engine.Parse())
                .Returns(new ParserResult(new string[]{"parsing error"}));
            IValidationEngine qp = mock.Object;

            var syntacticallyCorrectConstraint = new SyntacticallyCorrectConstraint() { Engine = qp };

            //Method under test
            var res = syntacticallyCorrectConstraint.Matches(new SqlCommand());

            //Test conclusion            
            Assert.That(res, Is.False);
        }

    }
}
