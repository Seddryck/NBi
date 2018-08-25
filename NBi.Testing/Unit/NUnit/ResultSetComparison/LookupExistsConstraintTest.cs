using Moq;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
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
            var child = new ResultSet();
            child.Load("a;b;1");
            var childMock = new Mock<IResultSetService>();
            childMock.Setup(s => s.Execute())
                .Returns(child);
            var childService = childMock.Object;

            var parent = new ResultSet();
            parent.Load("a;b");
            var parentMock = new Mock<IResultSetService>();
            parentMock.Setup(s => s.Execute())
                .Returns(parent);
            var parentService = parentMock.Object;

            var mappings = new ColumnMappingCollection()
            {
                new ColumnMapping(new ColumnPositionIdentifier(0), ColumnType.Text),
                new ColumnMapping(new ColumnPositionIdentifier(1), ColumnType.Text),
            };

            var lookupExists = new LookupExistsConstraint(parentService);
            lookupExists = lookupExists.Using(mappings);

            //Method under test
            lookupExists.Matches(childService);

            //Test conclusion            
            childMock.Verify(s => s.Execute(), Times.Once());
            parentMock.Verify(s => s.Execute(), Times.Once());
        }

        [Test]
        public void Matches_ReferenceAnalyzer_CallToExecuteOnce()
        {
            var child = new ResultSet();
            child.Load("a;b;1");
            var childMock = new Mock<IResultSetService>();
            childMock.Setup(s => s.Execute())
                .Returns(child);
            var childService = childMock.Object;

            var parent = new ResultSet();
            parent.Load("a;b");
            var parentMock = new Mock<IResultSetService>();
            parentMock.Setup(s => s.Execute())
                .Returns(parent);
            var parentService = parentMock.Object;

            var mappings = new ColumnMappingCollection()
            {
                new ColumnMapping(new ColumnPositionIdentifier(0), ColumnType.Text),
                new ColumnMapping(new ColumnPositionIdentifier(1), ColumnType.Text),
            };

            var lookupExists = new LookupExistsConstraint(parentService);
            var analyzer = new Mock<LookupAnalyzer>(mappings);
            analyzer.Setup(x => x.Execute(It.IsAny<ResultSet>(), It.IsAny<ResultSet>())).Returns(new LookupViolations());
            lookupExists.Engine = analyzer.Object;

            //Method under test
            lookupExists.Matches(childService);

            //Test conclusion            
            analyzer.Verify(x => x.Execute(child, parent), Times.Once());
        }
    }
}
