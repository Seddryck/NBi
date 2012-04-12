using System.Xml.Schema;
using Moq;
using NBi.Core;
using NBi.Core.Analysis.Query;
using NBi.NUnit;
using NUnit.Framework;

namespace NBi.Testing.NUnit
{
    [TestFixture]
    public class EqualsToConstraintTest
    {
        [Test]
        public void EqualsToConstraint_NUnitAssertThat_EngineCalledOnce()
        {
            var expectedPath = "C:\\ExpectedPath";
            var mock = new Mock<IResultSetComparer>();

            mock.Setup(engine => engine.Validate(It.IsAny<string>()))
                .Returns(Result.Success());
            IResultSetComparer rsc = mock.Object;

            var equalsToConstraint = new EqualsToConstraint(rsc);

            //Method under test
            Assert.That(expectedPath, equalsToConstraint);

            //Test conclusion            
            mock.Verify(engine => engine.Validate(expectedPath), Times.Once());
        }
    }
}
