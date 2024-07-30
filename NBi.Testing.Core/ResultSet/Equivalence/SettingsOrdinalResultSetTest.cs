#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet;
using NUnit.Framework;
using NBi.Core.Scalar.Comparer;
using NBi.Core.ResultSet.Equivalence;
#endregion

namespace NBi.Core.Testing.ResultSet
{
    [TestFixture]
    public class SettingsIndexResultSetTest
    {
        [Test]
        public void GetColumnType_EqualToAndColumnsDefinedCorrectResult()
        {
            var builder = new SettingsEquivalerBuilder();

            builder.Setup(SettingsOrdinalResultSet.KeysChoice.AllExpectLast, SettingsOrdinalResultSet.ValuesChoice.Last);
            builder.Setup(BuildColumns());
            builder.Build();
            //Get the Settings
            var settings = builder.GetSettings();
        
            Assert.That(settings, Is.TypeOf<SettingsOrdinalResultSet>());
            var actual = settings as SettingsOrdinalResultSet;
            actual!.ApplyTo(10);

            //Assertion
            Assert.That(actual.GetColumnType(0), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(1), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(2), Is.EqualTo(ColumnType.Text)); //By Default a key column is Text
            Assert.That(actual.GetColumnType(3), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(4), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(5), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(6), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(7), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(8), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(9), Is.EqualTo(ColumnType.Numeric));
        }
        
        [Test]
        public void GetTolerance_EqualToAndColumnsDefinedCorrectResult()
        {
            var builder = new SettingsEquivalerBuilder();

            builder.Setup(SettingsOrdinalResultSet.KeysChoice.AllExpectLast, SettingsOrdinalResultSet.ValuesChoice.Last);
            builder.Setup(BuildColumns());
            builder.Setup(ColumnType.Numeric, "100");
            builder.Build();
            //Get the Settings
            var settings = builder.GetSettings();

            Assert.That(settings, Is.TypeOf<SettingsOrdinalResultSet>());
            var actual = settings as SettingsOrdinalResultSet;
            actual!.ApplyTo(10);

            //apply specific value
            Assert.That(actual.GetTolerance(1), Is.TypeOf<NumericAbsoluteTolerance>());
            Assert.That((actual.GetTolerance(1) as NumericAbsoluteTolerance)!.Side, Is.EqualTo(SideTolerance.Both));
            Assert.That((actual.GetTolerance(1) as NumericAbsoluteTolerance)!.Value, Is.EqualTo(1));

            //apply default value
            Assert.That(Tolerance.IsNullOrNone(actual.GetTolerance(2))); //We haven't a Numeric column

            Assert.That(actual.GetTolerance(4), Is.TypeOf<NumericAbsoluteTolerance>());
            Assert.That((actual.GetTolerance(4) as NumericAbsoluteTolerance)!.Side, Is.EqualTo(SideTolerance.Both));
            Assert.That((actual.GetTolerance(4) as NumericAbsoluteTolerance)!.Value, Is.EqualTo(100));

            Assert.That(actual.GetTolerance(9), Is.TypeOf<NumericAbsoluteTolerance>());
            Assert.That((actual.GetTolerance(9) as NumericAbsoluteTolerance)!.Side, Is.EqualTo(SideTolerance.Both));
            Assert.That((actual.GetTolerance(9) as NumericAbsoluteTolerance)!.Value, Is.EqualTo(100));
        }
        
        [Test]
        public void GetColumnRole_EqualToAndColumnsDefinedCorrectResult()
        {
            var builder = new SettingsEquivalerBuilder();

            builder.Setup(SettingsOrdinalResultSet.KeysChoice.AllExpectLast, SettingsOrdinalResultSet.ValuesChoice.Last);
            builder.Setup(BuildColumns());
            builder.Build();
            //Get the Settings
            var settings = builder.GetSettings();
            
            Assert.That(settings, Is.TypeOf<SettingsOrdinalResultSet>());
            var actual = settings as SettingsOrdinalResultSet;
            actual!.ApplyTo(10);

            //Assertion
            Assert.That(actual.GetColumnRole(0), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(1), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(2), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(3), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(4), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(5), Is.EqualTo(ColumnRole.Ignore));
            Assert.That(actual.GetColumnRole(6), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(7), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(8), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(9), Is.EqualTo(ColumnRole.Value));
        }
        
        private IReadOnlyList<IColumnDefinition> BuildColumns()
        {
            //Build a value column (numeric, specific tolerance)
            var column = new Column(new ColumnOrdinalIdentifier(1), ColumnRole.Value, ColumnType.Numeric, "1");
            //Build a value column (without info)
            var colLightXml = new Column(new ColumnOrdinalIdentifier(2), ColumnRole.Value, ColumnType.Text);
            //Build a value column (numeric)
            var col4Xml = new Column(new ColumnOrdinalIdentifier(4),ColumnRole.Value,ColumnType.Numeric);
            //Build a ignore column (without info)
            var colIgnoreXml = new Column(new ColumnOrdinalIdentifier(5), ColumnRole.Ignore, ColumnType.Text);
            //Add columns to definition
            var columns = new List<Column>()
            {
                column,
                colLightXml,
                col4Xml,
                colIgnoreXml,
            };

            return columns.Cast<IColumnDefinition>().ToList().AsReadOnly();
        }
    }
}
