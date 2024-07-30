#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet;
using NUnit.Framework;
using NBi.Core.ResultSet.Equivalence;
using NBi.Core.Scalar.Comparer;
#endregion

namespace NBi.Core.Testing.ResultSet.Equivalence
{
    [TestFixture]
    public class SettingsNameResultSetTest
    {
        [Test]
        public void GetColumnRole_EqualToAndColumnsDefinedName_CorrectResult()
        {
            var builder = new SettingsEquivalerBuilder();

            builder.Setup(["Zero", "Three", "Six", "Seven", "Eight"], ["Nine"]);
            builder.Setup(BuildColumns());
            builder.Build();
            //Get the Settings
            var settings = builder.GetSettings();

            Assert.That(settings, Is.TypeOf<SettingsNameResultSet>());
            var actual = settings as SettingsNameResultSet;

            //Assertion
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual!.GetColumnRole("Zero"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("One"), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Two"), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Three"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Four"), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Five"), Is.EqualTo(ColumnRole.Ignore));
            Assert.That(actual.GetColumnRole("Six"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Seven"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Eight"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Nine"), Is.EqualTo(ColumnRole.Value));
        }

        [Test]
        public void GetColumnType_EqualToAndColumnsDefinedName_CorrectResult()
        {
            var builder = new SettingsEquivalerBuilder();

            builder.Setup(["Zero", "Three", "Six", "Seven", "Eight"], ["Nine"]);
            builder.Setup(BuildColumns());
            builder.Build();
            //Get the Settings
            var settings = builder.GetSettings();

            Assert.That(settings, Is.TypeOf<SettingsNameResultSet>());
            var actual = settings as SettingsNameResultSet;

            //Assertion
            Assert.That(actual!.GetColumnType("Zero"), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType("One"), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Two"), Is.EqualTo(ColumnType.Text)); //By Default a column is Text
            Assert.That(actual.GetColumnType("Three"), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType("Four"), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Five"), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType("Six"), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType("Seven"), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType("Eight"), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType("Nine"), Is.EqualTo(ColumnType.Numeric));

        }

        [Test]
        public void GetTolerance_EqualToAndColumnsDefinedName_CorrectResult()
        {
            var builder = new SettingsEquivalerBuilder();

            builder.Setup(["Zero", "Three", "Six", "Seven", "Eight"], ["Nine"]);
            builder.Setup(BuildColumns());
            builder.Setup(ColumnType.Numeric, "100");
            builder.Build();
            //Get the Settings
            var settings = builder.GetSettings();

            Assert.That(settings, Is.TypeOf<SettingsNameResultSet>());
            var actual = settings as SettingsNameResultSet;

            //Assertion
            //apply specific value
            Assert.That(actual!.GetTolerance("One"), Is.TypeOf<NumericAbsoluteTolerance>());
            Assert.That((actual.GetTolerance("One") as NumericAbsoluteTolerance)!.Side, Is.EqualTo(SideTolerance.Both));
            Assert.That((actual.GetTolerance("One") as NumericAbsoluteTolerance)!.Value, Is.EqualTo(1));

            //apply default value
            Assert.That(Tolerance.IsNullOrNone(actual.GetTolerance("Two"))); //We haven't a Numeric column

            Assert.That(actual.GetTolerance("Four"), Is.TypeOf<NumericAbsoluteTolerance>());
            Assert.That((actual.GetTolerance("Four") as NumericAbsoluteTolerance)!.Side, Is.EqualTo(SideTolerance.Both));
            Assert.That((actual.GetTolerance("Four") as NumericAbsoluteTolerance)!.Value, Is.EqualTo(100));

            Assert.That(actual.GetTolerance("Nine"), Is.TypeOf<NumericAbsoluteTolerance>());
            Assert.That((actual.GetTolerance("Nine") as NumericAbsoluteTolerance)!.Side, Is.EqualTo(SideTolerance.Both));
            Assert.That((actual.GetTolerance("Nine") as NumericAbsoluteTolerance)!.Value, Is.EqualTo(100));
        }

        private IReadOnlyList<IColumnDefinition> BuildColumns()
        {
            //Build a value column (numeric, specific tolerance)
            var column = new Column(new ColumnNameIdentifier("One"), ColumnRole.Value, ColumnType.Numeric, "1");
            //Build a value column (without info)
            var colLightXml = new Column(new ColumnNameIdentifier("Two"), ColumnRole.Value, ColumnType.Text);
            //Build a value column (numeric)
            var col4Xml = new Column(new ColumnNameIdentifier("Four"), ColumnRole.Value, ColumnType.Numeric);
                        //Build a ignore column (without info)
            var colIgnoreXml = new Column(new ColumnNameIdentifier("Five"), ColumnRole.Ignore, ColumnType.Text);
          
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
