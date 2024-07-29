using Moq;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Scalar.Comparer
{
    public class ToleranceFactoryTest
    {
        [Test]
        [TestCase(ColumnRole.Key)]
        [TestCase(ColumnRole.Value)]
        [TestCase(ColumnRole.Ignore)]
        public void Instantiate_NoToleranceDefinedColumnDefinition_InstantiatedToNullOrNone(ColumnRole columnRole)
        {
            var colDef = Mock.Of<IColumnDefinition>(
                x => x.Identifier == new ColumnOrdinalIdentifier(0)
                && x.Role == columnRole
                && x.Tolerance == string.Empty
                );
            var tolerance = new ToleranceFactory().Instantiate(colDef);
            
            Assert.That(Tolerance.IsNullOrNone(tolerance), Is.True);
        }

        [Test]
        [TestCase(ColumnType.Text)]
        [TestCase(ColumnType.Numeric)]
        [TestCase(ColumnType.DateTime)]
        [TestCase(ColumnType.Boolean)]
        public void Instantiate_NoToleranceDefinedType_InstantiatedToNullOrNone(ColumnType columnType)
        {
            var tolerance = new ToleranceFactory().Instantiate(columnType, string.Empty);
            Assert.That(Tolerance.IsNullOrNone(tolerance), Is.True);
        }
    }
}
