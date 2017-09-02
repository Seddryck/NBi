using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core.ResultSet;
using NBi.NUnit.Query;
using NUnit.Framework;
using NBi.Core;
using NBi.NUnit.ResultSetComparison;

namespace NBi.Testing.Unit.NUnit.ResultSetComparison
{
    [TestFixture]
    public class SupersetOfConstraintTest
    {
        [Test]
        public void Matches_AnyResultSets_EngineCalledOnceResultSetBuilderTwice()
        {
            var rs = new ResultSet();
            rs.Load("a;b;c");
            var cmd = new SqlCommand();

            var rsbMock = new Mock<ResultSetBuilder>();
            rsbMock.Setup(engine => engine.Build(It.IsAny<object>()))
                .Returns(rs);
            var rsb = rsbMock.Object;

            var rscMock = new Mock<IResultSetComparer>();
            rscMock.Setup(engine => engine.Compare(It.IsAny<ResultSet>(), It.IsAny<ResultSet>()))
                .Returns(new ResultSetCompareResult() { Difference = ResultSetDifferenceType.None });
            var rsc = rscMock.Object;

            var supersetOfConstraint = new SupersetOfConstraint(rs) {Engine = rsc, ResultSetBuilder=rsb };
            supersetOfConstraint.ResultSetBuilder = rsb;

            //Method under test
            supersetOfConstraint.Matches(cmd);

            //Test conclusion            
            //Test conclusion            
            rscMock.Verify(engine => engine.Compare(It.IsAny<ResultSet>(), It.IsAny<ResultSet>()), Times.Once());
            rsbMock.Verify(engine => engine.Build(It.IsAny<object>()), Times.Exactly(2));
        }

        [Test]
        public void Matches_IDbCommandAsActualAndPathAsExpectation_ResulSetBuildersCreateResultSetsUsingActualAndExpectationProvided()
        {
            var rsActual = new ResultSet();
            rsActual.Load("a;b;1");
            var rsExpect = new ResultSet();
            rsExpect.Load("x;y;2");
            var cmd = new SqlCommand();

            var rsbMock = new Mock<IResultSetBuilder>();
            rsbMock.Setup(engine => engine.Build(cmd))
                .Returns(rsActual);
            rsbMock.Setup(engine => engine.Build("my path for expectation"))
                .Returns(rsExpect);
            var rsb = rsbMock.Object;

            var supersetOfConstraint = new SupersetOfConstraint("my path for expectation") {ResultSetBuilder = rsb };

            //Method under test
            supersetOfConstraint.Matches(cmd);

            //Test conclusion            
            //Test conclusion            
            rsbMock.Verify(engine => engine.Build(cmd), Times.Once());
            rsbMock.Verify(engine => engine.Build("my path for expectation"), Times.Once());
        }
        

        [Test]
        public void Matches_AnyIDbCommandAsActualAndAnyPathAsExpectation_EngineCompareTheTwoResultSetsPreviouslyCreated()
        {
            var rsActual = new ResultSet(); 
            rsActual.Load("a;b;c");
            var rsExpect = new ResultSet();
            rsExpect.Load("x;y;z");
            var cmd = new SqlCommand();

            var rsbStub = new Mock<IResultSetBuilder>();
            rsbStub.Setup(engine => engine.Build(It.IsAny<IDbCommand>()))
                .Returns(rsActual);
            rsbStub.Setup(engine => engine.Build(It.IsAny<string>()))
                .Returns(rsExpect);                   

            var rsbFake = rsbStub.Object;

            var rscMock = new Mock<IResultSetComparer>();
            rscMock.Setup(engine => engine.Compare(rsActual, rsExpect))
                .Returns(ResultSetCompareResult.NotMatching);
            var rsc = rscMock.Object;

            var supersetOfConstraint = new SupersetOfConstraint("my path for expectation") {ResultSetBuilder = rsbFake, Engine = rsc };

            //Method under test
            supersetOfConstraint.Matches(cmd);

            //Test conclusion            
            //Test conclusion            
            rscMock.Verify(engine => engine.Compare(rsActual, rsExpect), Times.Once());
        }
        [Test]
        public void Matches_TwoIdenticalResultSets_ReturnTrue()
        {
            var rs = new ResultSet();
            rs.Load("a;X;1");

            var cmd = new SqlCommand();

            var rsbMock = new Mock<IResultSetBuilder>();
            rsbMock.Setup(engine => engine.Build(It.IsAny<IDbCommand>()))
                .Returns(rs);
            rsbMock.Setup(engine => engine.Build(rs))
                .Returns(rs);
            var rsb = rsbMock.Object;

            var supersetOfConstraint = new SupersetOfConstraint(rs) { ResultSetBuilder = rsb };

            //Method under test
            var res = supersetOfConstraint.Matches(cmd);

            //Test conclusion            
            rsbMock.Verify(engine => engine.Build(rs), Times.Once());
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_TwoDifferentResultSets_ReturnFalse()
        {
            var rsActual = new ResultSet();
            rsActual.Load("a;b;1");
            var rsExpect = new ResultSet();
            rsExpect.Load("a;X;1");

            var cmd = new SqlCommand();

            var rsbMock = new Mock<IResultSetBuilder>();
            rsbMock.Setup(engine => engine.Build(It.IsAny<IDbCommand>()))
                .Returns(rsActual);
            rsbMock.Setup(engine => engine.Build(rsExpect))
                .Returns(rsExpect);
            var rsb = rsbMock.Object;

            var supersetOfConstraint = new SupersetOfConstraint(rsExpect) {ResultSetBuilder = rsb };

            //Method under test
            var res = supersetOfConstraint.Matches(cmd);

            //Test conclusion            
            rsbMock.Verify(engine => engine.Build(rsExpect), Times.Once());
            Assert.That(res, Is.False);
        }
    }
}
