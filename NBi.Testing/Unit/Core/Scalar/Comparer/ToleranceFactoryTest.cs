using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Xml.Items.ResultSet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Scalar.Comparer
{
    public class ToleranceFactoryTest
    {
        [Test]
        [TestCase(ColumnRole.Key)]
        [TestCase(ColumnRole.Value)]
        [TestCase(ColumnRole.Ignore)]
        public void Instantiate_NoToleranceDefined_InstantiatedToNullOrNone(ColumnRole columnRole)
        {
            var colDef = new ColumnDefinitionXml()
            {
                Index = 0,
                Role = columnRole,
                Tolerance = string.Empty
            };
            var tolerance = new ToleranceFactory().Instantiate(colDef);
            
            Assert.That(Tolerance.IsNullOrNone(tolerance), Is.True);
        }

        [Test]
        [TestCase(ColumnType.Text)]
        [TestCase(ColumnType.Numeric)]
        [TestCase(ColumnType.DateTime)]
        [TestCase(ColumnType.Boolean)]
        public void Instantiate_NoToleranceDefined_InstantiatedToNullOrNone(ColumnType columnType)
        {
            var tolerance = new ToleranceFactory().Instantiate(columnType, string.Empty);
            Assert.That(Tolerance.IsNullOrNone(tolerance), Is.True);
        }
    }
}
