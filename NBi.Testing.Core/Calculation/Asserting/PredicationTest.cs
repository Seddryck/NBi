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
        [Test]
        public void Execute_Or_StopOnFirstTrue()
        {
            var predicateMock1 = new Mock<Exssif.IPredicate>();
            predicateMock1.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(true);
            var predicateMock2 = new Mock<Exssif.IPredicate>();
            predicateMock2.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(false);
            var predicateMock3 = new Mock<Exssif.IPredicate>();
            predicateMock3.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(false);
            var combined = new Exssif.PredicateCombiner()
                                .With(predicateMock1.Object)
                                .Or(predicateMock2.Object)
                                .Or(predicateMock3.Object)
                                .Build();

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new Predicate(combined), new ColumnOrdinalIdentifier(0));

            var dt = new DataTable();
            var row = dt.NewRow();
            row.ItemArray = [0];
            dt.Rows.Add(row);
            predication.Execute(Context.None);

            predicateMock1.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock2.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Never);
            predicateMock3.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Never);
        }

        [Test]
        public void Execute_And_StopOnFirstFalse()
        {
            var predicateMock1 = new Mock<Exssif.IPredicate>();
            predicateMock1.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(true);
            var predicateMock2 = new Mock<Exssif.IPredicate>();
            predicateMock2.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(false);
            var predicateMock3 = new Mock<Exssif.IPredicate>();
            predicateMock3.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(false);
            var combined = new Exssif.PredicateCombiner()
                                .With(predicateMock1.Object)
                                .And(predicateMock2.Object)
                                .And(predicateMock3.Object)
                                .Build();

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new Predicate(combined), new ColumnOrdinalIdentifier(0));

            var dt = new DataTable();
            var row = dt.NewRow();
            row.ItemArray = [0];
            dt.Rows.Add(row);
            predication.Execute(Context.None);

            predicateMock1.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock2.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock3.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Never);
        }

        [Test]
        public void Execute_XOr_EvaluateAll()
        {
            var predicateMock1 = new Mock<Exssif.IPredicate>();
            predicateMock1.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(true);
            var predicateMock2 = new Mock<Exssif.IPredicate>();
            predicateMock2.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(false);
            var predicateMock3 = new Mock<Exssif.IPredicate>();
            predicateMock3.Setup(x => x.Evaluate(It.IsAny<object>())).Returns(false);
            var combined = new Exssif.PredicateCombiner()
                                .With(predicateMock1.Object)
                                .Xor(predicateMock2.Object)
                                .Xor(predicateMock3.Object)
                                .Build();

            var factory = new PredicationFactory();
            var predication = factory.Instantiate(new Predicate(combined), new ColumnOrdinalIdentifier(0));

            var dt = new DataTable();
            var row = dt.NewRow();
            row.ItemArray = [0];
            dt.Rows.Add(row);
            predication.Execute(Context.None);

            predicateMock1.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock2.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
            predicateMock3.Verify(x => x.Evaluate(It.IsAny<object>()), Times.Once);
        }
    }
}