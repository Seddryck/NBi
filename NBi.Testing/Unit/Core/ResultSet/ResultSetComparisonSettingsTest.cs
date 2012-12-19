#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints;
using NBi.Xml.Constraints.EqualTo;
using NUnit.Framework;
using NBi.Core;

#endregion

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class ResultSetComparisonSettingsTest
    {
        [Test]
        public void GetLastColumnIndex_EqualToAndColumnsDefined_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.GetLastColumnIndex(), Is.EqualTo(9));
        }

        [Test]
        public void GetColumnRole_EqualToAndColumnsDefined_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.GetColumnRole(0), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(1), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(2), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(3), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(4), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(5), Is.EqualTo(ColumnRole.Ignore));
            Assert.That(actual.GetColumnRole(8), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(9), Is.EqualTo(ColumnRole.Value));
        }

        [Test]
        public void GetColumnType_EqualToAndColumnsDefined_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.GetColumnType(0), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(1), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(2), Is.EqualTo(ColumnType.Text)); //By Default a column is Text
            Assert.That(actual.GetColumnType(3), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(4), Is.EqualTo(ColumnType.Numeric)); 
            Assert.That(actual.GetColumnType(8), Is.EqualTo(ColumnType.Text));
            //default for a Value is Numeric
            Assert.That(actual.GetColumnType(9), Is.EqualTo(ColumnType.Numeric));

        }

        [Test]
        public void GetTolerance_EqualToAndColumnsDefined_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            //apply specific value
            Assert.That(actual.GetTolerance(1), Is.EqualTo(1));
            //apply default value
            Assert.That(actual.GetTolerance(2), Is.EqualTo(0)); //We haven't a Numeric column
            Assert.That(actual.GetTolerance(4), Is.EqualTo(100)); 
            Assert.That(actual.GetTolerance(9), Is.EqualTo(100));
        }

        [Test]
        public void IsKey_EqualToAndColumnsDefined_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.IsKey(0), Is.True);
            Assert.That(actual.IsKey(1), Is.False);
            Assert.That(actual.IsKey(2), Is.False);
            Assert.That(actual.IsKey(3), Is.True);
            Assert.That(actual.IsKey(4), Is.False);
            Assert.That(actual.IsKey(5), Is.False);
            Assert.That(actual.IsKey(8), Is.True);
            Assert.That(actual.IsKey(9), Is.False);
        }

        [Test]
        public void IsValue_EqualToAndColumnsDefined_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.IsValue(0), Is.False);
            Assert.That(actual.IsValue(1), Is.True);
            Assert.That(actual.IsValue(2), Is.True);
            Assert.That(actual.IsValue(3), Is.False);
            Assert.That(actual.IsValue(4), Is.True);
            Assert.That(actual.IsValue(5), Is.False);
            Assert.That(actual.IsValue(8), Is.False);
            Assert.That(actual.IsValue(9), Is.True);
        }

        [Test]
        public void IsNumeric_EqualToAndColumnsDefined_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.IsNumeric(0), Is.False);
            Assert.That(actual.IsNumeric(1), Is.True);
            Assert.That(actual.IsNumeric(2), Is.False); //By Default a Column is Textual
            Assert.That(actual.IsNumeric(3), Is.False);
            Assert.That(actual.IsNumeric(4), Is.True);
            Assert.That(actual.IsNumeric(8), Is.False);
            Assert.That(actual.IsNumeric(9), Is.True);
        }

        private ResultSetComparisonSettings BuildSettings()
        {
            //Buiding object used during test
            var xml = new EqualToXml();
            //default values/def
            xml.KeysDef = ResultSetComparisonSettings.KeysChoice.AllExpectLast;
            //default tolerance
            xml.Tolerance = 100;

            //Build a value column (numeric, specific tolerance)
            var colXml = new ColumnXml();
            colXml.Index = 1;
            colXml.Role = ColumnRole.Value;
            colXml.Type = ColumnType.Numeric;
            colXml.Tolerance = 1;

            //Build a value column (without info)
            var colLightXml = new ColumnXml();
            colLightXml.Index = 2;
            colLightXml.Role = ColumnRole.Value;

            //Build a value column (numeric)
            var col4Xml = new ColumnXml();
            col4Xml.Index = 4;
            col4Xml.Role = ColumnRole.Value;
            col4Xml.Type = ColumnType.Numeric;

            //Build a ignore column (without info)
            var colIgnoreXml = new ColumnXml();
            colIgnoreXml.Index = 5;
            colIgnoreXml.Role = ColumnRole.Ignore;

            //Add columns to definition
            var cols = new List<ColumnXml>();
            cols.Add(colXml);
            cols.Add(colLightXml);
            cols.Add(col4Xml);
            cols.Add(colIgnoreXml);
            xml.columnsDef = cols;

            //get settings
            var settings = xml.GetSettings();

            //Set the columnCount
            settings.ApplyTo(10);

            return settings;
        }

    }
}
