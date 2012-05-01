using System.Data;
using System.Data.OleDb;
using Moq;
using NBi.Core;
using NBi.Core.Analysis.Query;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.Unit.NUnit
{
    [TestFixture]
    public class EqualToConstraintTest
    {
        [Test]
        public void EqualToConstraint_NUnitAssertThatIDbCommand_EngineCalledOnce()
        {
            var mock = new Mock<IResultSetComparer>();
            mock.Setup(engine => engine.Validate(It.IsAny<IDbCommand>()))
                .Returns(Result.Success());
            IResultSetComparer rsc = mock.Object;

            var equalToConstraint = new EqualToConstraint(rsc);
            var cmd = new OleDbCommand();

            //Method under test
            Assert.That(cmd, equalToConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.Validate(cmd), Times.Once());
        }
    }
}
