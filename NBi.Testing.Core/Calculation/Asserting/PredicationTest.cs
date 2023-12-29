using NUnit.Framework;
using NBi.Core.Calculation;
using Moq;
using System.Data;
using NBi.Core.Calculation.Asserting;
using NBi.Core.Variable;
using Exssif = Expressif.Predicates;
using NBi.Core.ResultSet;

namespace NBi.Core.Testing.Calculation.Asserting
{
    public class PredicationTest
    {
        private Mock<Exssif.IPredicate> CreateMock(bool expected)
        {
            var mock = new Mock<Exssif.IPredicate>();
            mock.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(expected);
            return mock;
        }

        private Context CreateContext()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Index", typeof(int)));
            var row = dt.NewRow();
            row.ItemArray = [0];
            dt.Rows.Add(row);
            var context = Context.None;
            context.Switch(new DataRowResultSet(row));
            return context;
        }

        [Test]
        public void Execute_Or_StopOnFirstTrue()
        {
            (var predicateMock1, var predicateMock2, var predicateMock3) = (CreateMock(true), CreateMock(false), CreateMock(false));
            var combined = new Exssif.PredicateCombiner()
                                .With(predicateMock1.Object)
                                .Or(predicateMock2.Object)
                                .Or(predicateMock3.Object)
                                .Build();

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new Predicate(combined), new ColumnOrdinalIdentifier(0));
            predication.Execute(CreateContext());

            predicateMock1.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock2.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Never);
            predicateMock3.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Never);
        }

        [Test]
        public void Execute_And_StopOnFirstFalse()
        {
            (var predicateMock1, var predicateMock2, var predicateMock3) = (CreateMock(true), CreateMock(false), CreateMock(false));
            var combined = new Exssif.PredicateCombiner()
                                .With(predicateMock1.Object)
                                .And(predicateMock2.Object)
                                .And(predicateMock3.Object)
                                .Build();

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new Predicate(combined), new ColumnOrdinalIdentifier(0));
            predication.Execute(CreateContext());

            predicateMock1.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock2.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock3.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Never);
        }

        [Test]
        public void Execute_XOr_EvaluateAll()
        {
            (var predicateMock1, var predicateMock2, var predicateMock3) = (CreateMock(true), CreateMock(false), CreateMock(false));
            var combined = new Exssif.PredicateCombiner()
                                .With(predicateMock1.Object)
                                .Xor(predicateMock2.Object)
                                .Xor(predicateMock3.Object)
                                .Build();

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new Predicate(combined), new ColumnOrdinalIdentifier(0));
            predication.Execute(CreateContext());

            predicateMock1.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock2.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock3.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
        }
    }
}
