#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints;
using NBi.Xml.Items.ResultSet;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class SettingsResultSetComparisonByNameTest
    {
        
        [Test]
        public void GetColumnRole_EqualToAndColumnsDefinedName_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.GetColumnRole("Zero"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("One"), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Two"), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Three"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Four"), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Five"), Is.EqualTo(ColumnRole.Ignore));
            Assert.That(actual.GetColumnRole("Eight"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Nine"), Is.EqualTo(ColumnRole.Value));
        }
        
        [Test]
        public void GetColumnType_EqualToAndColumnsDefinedName_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.GetColumnType("Zero"), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType("One"), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Two"), Is.EqualTo(ColumnType.Text)); //By Default a column is Text
            Assert.That(actual.GetColumnType("Three"), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType("Four"), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Eight"), Is.EqualTo(ColumnType.Text));
            //default for a SettingsValue is Numeric
            Assert.That(actual.GetColumnType("Nine"), Is.EqualTo(ColumnType.Numeric));

        }
        
        [Test]
        public void GetTolerance_EqualToAndColumnsDefinedName_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            //apply specific value
            Assert.That(actual.GetTolerance("One").ValueString, Is.EqualTo("1"));
            //apply default value
            Assert.That(actual.GetTolerance("Two"), Is.Null); //We haven't a Numeric column
            Assert.That(actual.GetTolerance("Four").ValueString, Is.EqualTo("100"));
            Assert.That(actual.GetTolerance("Nine").ValueString, Is.EqualTo("100"));
        }
        
        [Test]
        public void IsKey_EqualToAndColumnsDefinedName_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.GetColumnRole("Zero"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("One"), Is.Not.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Two"), Is.Not.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Three"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Four"), Is.Not.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Five"), Is.Not.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Eight"), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole("Nine"), Is.Not.EqualTo(ColumnRole.Key));
        }
        
        [Test]
        public void IsValue_EqualToAndColumnsDefinedName_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.GetColumnRole("Zero"), Is.Not.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("One"), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Two"), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Three"), Is.Not.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Four"), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Five"), Is.Not.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Eight"), Is.Not.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole("Nine"), Is.EqualTo(ColumnRole.Value));
        }
        
        [Test]
        public void IsNumeric_EqualToAndColumnsDefinedName_CorrectResult()
        {
            //Get the Settings
            var actual = BuildSettings();

            //Assertion
            Assert.That(actual.GetColumnType("Zero"), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("One"), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Two"), Is.Not.EqualTo(ColumnType.Numeric)); //By Default a Column is Textual
            Assert.That(actual.GetColumnType("Three"), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Four"), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Eight"), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Nine"), Is.EqualTo(ColumnType.Numeric));
        }
        
        [Test]
        public void IsNumeric_EqualToAndColumnsDefinedWithValuesDefaultTypeName_CorrectResult()
        {
            //Get the Settings
            var xml = BuildEqualToXml();
            xml.ValuesDefaultType = ColumnType.DateTime;
            //get settings
            var actual = (xml.GetSettings() as SettingsResultSetComparisonByName);

            //Assertion
            Assert.That(actual.GetColumnType("Zero"), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("One"), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Two"), Is.Not.EqualTo(ColumnType.Numeric)); //By Default a Key column is Textual
            Assert.That(actual.GetColumnType("Two"), Is.Not.EqualTo(ColumnType.DateTime)); //By Default a Key column is Textual
            Assert.That(actual.GetColumnType("Two"), Is.Not.EqualTo(ColumnType.Boolean)); //By Default a Key column is Textual
            Assert.That(actual.GetColumnType("Three"), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Four"), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Eight"), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType("Nine"), Is.Not.EqualTo(ColumnType.Numeric)); //The default type for a Value column is dateTime
            Assert.That(actual.GetColumnType("Nine"), Is.EqualTo(ColumnType.DateTime)); //The default type for a Value column is dateTime
        }
        
        private EqualToXml BuildEqualToXml()
        {
            //Buiding object used during test
            var xml = new EqualToXml();
            //default values/def
            xml.KeyName = "Zero, Three, Six, Seven, Eight";
            //default values/def
            xml.ValueName = "Nine";
            //default values/def
            xml.ValuesDefaultType = ColumnType.Numeric;
            //default tolerance
            xml.Tolerance = "100";

            //Build a value column (numeric, specific tolerance)
            var colXml = new ColumnDefinitionXml();
            colXml.Name = "One";
            colXml.Role = ColumnRole.Value;
            colXml.Type = ColumnType.Numeric;
            colXml.Tolerance = "1";

            //Build a value column (without info)
            var colLightXml = new ColumnDefinitionXml();
            colLightXml.Name = "Two";
            colLightXml.Role = ColumnRole.Value;

            //Build a value column (numeric)
            var col4Xml = new ColumnDefinitionXml();
            col4Xml.Name = "Four";
            col4Xml.Role = ColumnRole.Value;
            col4Xml.Type = ColumnType.Numeric;

            //Build a ignore column (without info)
            var colIgnoreXml = new ColumnDefinitionXml();
            colIgnoreXml.Name = "Five";
            colIgnoreXml.Role = ColumnRole.Ignore;

            //Add columns to definition
            var cols = new List<ColumnDefinitionXml>();
            cols.Add(colXml);
            cols.Add(colLightXml);
            cols.Add(col4Xml);
            cols.Add(colIgnoreXml);
            xml.columnsDef = cols;

            return xml;
        }
        
        private SettingsResultSetComparisonByName BuildSettings()
        {
            var xml = BuildEqualToXml();
            //get settings
            var settings = xml.GetSettings();
            
            return (settings as SettingsResultSetComparisonByName);
        }

    }
}
