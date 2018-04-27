using Moq;
using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using NBi.Core.ResultSet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Calculation
{
    public class PredicateTwoOperandsTest
    {
        [Test]
        [TestCase(ComparerType.Modulo, 10, 5, 0)]
        [TestCase(ComparerType.Modulo, 10, 4, 2)]
        public void Compare_Numeric_Success(ComparerType comparerType, object x, object sop, object reference)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns(reference);
            predicate.As<ISecondOperandPredicateInfo>().SetupGet(p => p.SecondOperand).Returns(sop);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Execute(x), Is.True);
        }

        [Test]
        [TestCase(ComparerType.Modulo, 10, 6, 0)]
        [TestCase(ComparerType.Modulo, 10, 5, 1)]
        public void Compare_Numeric_Failure(ComparerType comparerType, object x, object sop, object reference)
        {
            var predicate = new Mock<IPredicateInfo>();
            predicate.SetupGet(p => p.ColumnType).Returns(ColumnType.Numeric);
            predicate.SetupGet(p => p.ComparerType).Returns(comparerType);
            predicate.As<IReferencePredicateInfo>().SetupGet(p => p.Reference).Returns(reference);
            predicate.As<ISecondOperandPredicateInfo>().SetupGet(p => p.SecondOperand).Returns(sop);

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(predicate.Object);
            Assert.That(comparer.Execute(x), Is.False);
        }
    }
}
