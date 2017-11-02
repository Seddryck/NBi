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
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType == ColumnType.Numeric
                    && i.ComparerType == comparerType
                    && i.SecondOperand == sop
                    && i.Reference == reference
                );

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(info);
            Assert.That(comparer.Apply(x), Is.True);
        }

        [Test]
        [TestCase(ComparerType.Modulo, 10, 6, 0)]
        [TestCase(ComparerType.Modulo, 10, 5, 1)]
        public void Compare_Numeric_Failure(ComparerType comparerType, object x, object sop, object reference)
        {
            var info = Mock.Of<IPredicateInfo>(
                    i => i.ColumnType == ColumnType.Numeric
                    && i.ComparerType == comparerType
                    && i.SecondOperand == sop
                    && i.Reference == reference
                );

            var factory = new PredicateFactory();
            var comparer = factory.Instantiate(info);
            Assert.That(comparer.Apply(x), Is.False);
        }
    }
}
