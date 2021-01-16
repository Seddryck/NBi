using Moq;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility.Resolving;
using NBi.NUnit.ResultSetComparison;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.NUnit.ResultSetComparison
{
    public class LookupExistsConstraintTest
    {
        [Test]
        public void Matches_ResultSetService_CallToExecuteOnce()
        {
            var sut = new ResultSet();
            sut.Load("a;b;1");
            var sutMock = new Mock<IResultSetResolver>();
            sutMock.Setup(s => s.Execute())
                .Returns(sut);
            var sutService = sutMock.Object;

            var assert = new ResultSet();
            assert.Load("a;b");
            var assertMock = new Mock<IResultSetResolver>();
            assertMock.Setup(s => s.Execute())
                .Returns(assert);
            var assertService = assertMock.Object;

            var mappings = new ColumnMappingCollection()
            {
                new ColumnMapping(new ColumnOrdinalIdentifier(0), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(1), ColumnType.Text),
            };

            var lookupExists = new LookupExistsConstraint(assertService);
            lookupExists = lookupExists.Using(mappings);

            //Method under test
            lookupExists.Matches(sutService);

            //Test conclusion            
            sutMock.Verify(s => s.Execute(), Times.Once());
            assertMock.Verify(s => s.Execute(), Times.Once());
        }

        [Test]
        public void Matches_ReferenceAnalyzer_CallToExecuteOnce()
        {
            var sut = new ResultSet();
            sut.Load("a;b;1");
            var sutMock = new Mock<IResultSetResolver>();
            sutMock.Setup(s => s.Execute())
                .Returns(sut);
            var sutService = sutMock.Object;

            var assert = new ResultSet();
            assert.Load("a;b");
            var assertMock = new Mock<IResultSetResolver>();
            assertMock.Setup(s => s.Execute())
                .Returns(assert);
            var assertService = assertMock.Object;

            var mappings = new ColumnMappingCollection()
            {
                new ColumnMapping(new ColumnOrdinalIdentifier(0), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(1), ColumnType.Text),
            };

            var lookupExists = new LookupExistsConstraint(assertService);
            var analyzer = new Mock<LookupExistsAnalyzer>(mappings);
            analyzer.Setup(x => x.Execute(It.IsAny<ResultSet>(), It.IsAny<ResultSet>())).Returns(new LookupExistsViolationCollection(null));
            lookupExists.Engine = analyzer.Object;

            //Method under test
            lookupExists.Matches(sutService);

            //Test conclusion            
            analyzer.Verify(x => x.Execute(sut, assert), Times.Once());
        }
    }
}
