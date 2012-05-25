using System.Data.SqlClient;
using Moq;
using NBi.Core.Query;
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
        public void Matches_AnyIDbCommand_EngineParseCalledOnce()
        {

            var mock = new Mock<IQueryParser>();
            mock.Setup(engine => engine.Parse())
                .Returns(ParserResult.NoParsingError());
            IQueryParser qp = mock.Object;

            var syntacticallyCorrectConstraint = new NBi.NUnit.SyntacticallyCorrectConstraint() { Engine = qp };

            //Method under test
            syntacticallyCorrectConstraint.Matches(new SqlCommand());

            //Test conclusion            
            mock.Verify(engine => engine.Parse(), Times.Once());
        }

        [Test]
        public void Matches_NoParsingError_ReturnTrue()
        {

            var mock = new Mock<IQueryParser>();
            mock.Setup(engine => engine.Parse())
                .Returns(ParserResult.NoParsingError());
            IQueryParser qp = mock.Object;

            var syntacticallyCorrectConstraint = new NBi.NUnit.SyntacticallyCorrectConstraint() { Engine = qp };

            //Method under test
            var res = syntacticallyCorrectConstraint.Matches(new SqlCommand());

            //Test conclusion            
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_WithParsingError_ReturnFalse()
        {

            var mock = new Mock<IQueryParser>();
            mock.Setup(engine => engine.Parse())
                .Returns(new ParserResult(new string[]{"parsing error"}));
            IQueryParser qp = mock.Object;

            var syntacticallyCorrectConstraint = new NBi.NUnit.SyntacticallyCorrectConstraint() { Engine = qp };

            //Method under test
            var res = syntacticallyCorrectConstraint.Matches(new SqlCommand());

            //Test conclusion            
            Assert.That(res, Is.False);
        }

    }
}
