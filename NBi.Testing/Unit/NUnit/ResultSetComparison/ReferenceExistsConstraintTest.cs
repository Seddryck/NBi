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
    public class ReferenceExistsConstraintTest
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
                new ColumnMapping("#0", "#0", ColumnType.Text),
                new ColumnMapping("#1", "#1", ColumnType.Text),
            };


            var referenceExists = new ReferenceExistsConstraint(parentService);
            referenceExists = referenceExists.Using(mappings);

            //Method under test
            referenceExists.Matches(childService);

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
                new ColumnMapping("#0", "#0", ColumnType.Text),
                new ColumnMapping("#1", "#1", ColumnType.Text),
            };


            var referenceExists = new ReferenceExistsConstraint(parentService);
            var analyzer = new Mock<ReferenceAnalyzer>(mappings);
            analyzer.Setup(x => x.Execute(It.IsAny<ResultSet>(), It.IsAny<ResultSet>())).Returns(new ReferenceViolations());
            referenceExists.Engine = analyzer.Object;

            //Method under test
            referenceExists.Matches(childService);

            //Test conclusion            
            analyzer.Verify(x => x.Execute(child, parent), Times.Once());
        }
    }
}
