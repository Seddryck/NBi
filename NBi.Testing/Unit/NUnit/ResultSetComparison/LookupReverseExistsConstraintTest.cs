using Moq;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
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
    public class LookupReverseExistsConstraintTest
    {
        [Test]
        public void Matches_ResultSetService_CallToExecuteOnce()
        {
            var candidate = new DataTableResultSet();
            candidate.Load("a;b;1");
            var sutMock = new Mock<IResultSetService>();
            sutMock.Setup(s => s.Execute())
                .Returns(candidate);
            var candidateService = sutMock.Object;

            var assert = new DataTableResultSet();
            assert.Load("a;b");
            var assertMock = new Mock<IResultSetService>();
            assertMock.Setup(s => s.Execute())
                .Returns(assert);
            var assertService = assertMock.Object;

            var mappings = new ColumnMappingCollection()
            {
                new ColumnMapping(new ColumnOrdinalIdentifier(0), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(1), ColumnType.Text),
            };

            var lookupExists = new LookupReverseExistsConstraint(candidateService);
            lookupExists = lookupExists.Using(mappings);

            //Method under test
            lookupExists.Matches(assertService);

            //Test conclusion            
            sutMock.Verify(s => s.Execute(), Times.Once());
            assertMock.Verify(s => s.Execute(), Times.Once());
        }

        [Test]
        public void Matches_LookupAnalyzer_CallToExecuteOnce()
        {
            var sut = new DataTableResultSet();
            sut.Load("a;b;1");
            var sutMock = new Mock<IResultSetService>();
            sutMock.Setup(s => s.Execute())
                .Returns(sut);
            var sutService = sutMock.Object;

            var assert = new DataTableResultSet();
            assert.Load("a;b");
            var assertMock = new Mock<IResultSetService>();
            assertMock.Setup(s => s.Execute())
                .Returns(assert);
            var assertService = assertMock.Object;

            var mappings = new ColumnMappingCollection()
            {
                new ColumnMapping(new ColumnOrdinalIdentifier(0), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(1), ColumnType.Text),
            };

            var lookupExists = new LookupReverseExistsConstraint(assertService);
            var analyzer = new Mock<LookupExistsAnalyzer>(mappings);
            analyzer.Setup(x => x.Execute(It.IsAny<IResultSet>(), It.IsAny<IResultSet>())).Returns(new LookupExistsViolationCollection(null));
            lookupExists.Engine = analyzer.Object;

            //Method under test
            lookupExists.Matches(sutService);

            //Test conclusion            
            analyzer.Verify(x => x.Execute(assert, sut), Times.Once());
        }
    }
}
