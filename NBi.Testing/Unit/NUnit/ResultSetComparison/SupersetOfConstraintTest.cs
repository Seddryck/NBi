using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core.ResultSet;
using NBi.NUnit.Query;
using NUnit.Framework;
using NBi.Core;
using NBi.NUnit.ResultSetComparison;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet.Equivalence;
using NBi.Extensibility.Resolving;

namespace NBi.Testing.Unit.NUnit.ResultSetComparison
{
    [TestFixture]
    public class SupersetOfConstraintTest
    {
        [Test]
        public void Matches_AnyServices_EachCalledOnce()
        {
            var rs = new ResultSet();
            rs.Load("a;b;c");

            var expectedServiceMock = new Mock<IResultSetResolver>();
            expectedServiceMock.Setup(s => s.Execute())
                .Returns(rs);
            var expectedService = expectedServiceMock.Object;

            var actualServiceMock = new Mock<IResultSetResolver>();
            actualServiceMock.Setup(s => s.Execute())
                .Returns(rs);
            var actualService = actualServiceMock.Object;

            var rscMock = new Mock<IEquivaler>();
            rscMock.Setup(engine => engine.Compare(It.IsAny<ResultSet>(), It.IsAny<ResultSet>()))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.None });
            var rsc = rscMock.Object;

            var supersetOfConstraint = new SupersetOfConstraint(expectedService) { Engine = rsc };

            //Method under test
            supersetOfConstraint.Matches(actualService);

            //Test conclusion            
            rscMock.Verify(engine => engine.Compare(It.IsAny<ResultSet>(), It.IsAny<ResultSet>()), Times.Once());
            expectedServiceMock.Verify(s => s.Execute(), Times.Once);
            actualServiceMock.Verify(s => s.Execute(), Times.Once);
        }

        [Test]
        public void Matches_AnyServices_TheirResultsAreCompared()
        {
            var expectedRs = new ResultSet();
            expectedRs.Load("a;b;c");

            var actualRs = new ResultSet();
            actualRs.Load("x;y;z");

            var expectedServiceMock = new Mock<IResultSetResolver>();
            expectedServiceMock.Setup(s => s.Execute())
                .Returns(expectedRs);
            var expectedService = expectedServiceMock.Object;

            var actualServiceMock = new Mock<IResultSetResolver>();
            actualServiceMock.Setup(s => s.Execute())
                .Returns(actualRs);
            var actualService = actualServiceMock.Object;

            var rscMock = new Mock<IEquivaler>();
            rscMock.Setup(engine => engine.Compare(It.IsAny<ResultSet>(), It.IsAny<ResultSet>()))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.Content });
            var rsc = rscMock.Object;

            var supersetOfConstraint = new SupersetOfConstraint(expectedService) { Engine = rsc };

            //Method under test
            supersetOfConstraint.Matches(actualService);

            //Test conclusion            
            rscMock.Verify(engine => engine.Compare(actualRs, expectedRs), Times.Once());
        }

        [Test]
        public void Matches_TwoIdenticalResultSets_ReturnTrue()
        {
            var rs = new ResultSet();
            rs.Load("a;b;c");

            var expectedServiceMock = new Mock<IResultSetResolver>();
            expectedServiceMock.Setup(s => s.Execute())
                .Returns(rs);
            var expectedService = expectedServiceMock.Object;

            var actualServiceMock = new Mock<IResultSetResolver>();
            actualServiceMock.Setup(s => s.Execute())
                .Returns(rs);
            var actualService = actualServiceMock.Object;

            var rscMock = new Mock<IEquivaler>();
            rscMock.Setup(engine => engine.Compare(rs, rs))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.None });
            var rsc = rscMock.Object;

            var supersetOfConstraint = new SupersetOfConstraint(expectedService) { Engine = rsc };

            //Method under test
            var result = supersetOfConstraint.Matches(actualService);

            //Test conclusion            
            Assert.That(result, Is.True);
        }

        [Test]
        public void Matches_TwoDifferentResultSets_ReturnFalse()
        {
            var expectedRs = new ResultSet();
            expectedRs.Load("a;b;c");

            var actualRs = new ResultSet();
            actualRs.Load("x;y;z");

            var expectedServiceMock = new Mock<IResultSetResolver>();
            expectedServiceMock.Setup(s => s.Execute())
                .Returns(expectedRs);
            var expectedService = expectedServiceMock.Object;

            var actualServiceMock = new Mock<IResultSetResolver>();
            actualServiceMock.Setup(s => s.Execute())
                .Returns(actualRs);
            var actualService = actualServiceMock.Object;

            var rscMock = new Mock<IEquivaler>();
            rscMock.Setup(engine => engine.Compare(actualRs, expectedRs))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.Content });
            var rsc = rscMock.Object;

            var supersetOfConstraint = new SupersetOfConstraint(expectedService) { Engine = rsc };

            //Method under test
            var result = supersetOfConstraint.Matches(actualService);

            //Test conclusion            
            Assert.That(result, Is.False);
        }
    }
}
