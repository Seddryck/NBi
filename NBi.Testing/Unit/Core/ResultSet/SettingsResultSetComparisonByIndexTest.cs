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
    public class SettingsResultSetComparisonByIndexTest
    {
        [Test]
        public void GetColumnType_EqualToAndColumnsDefinedCorrectResult()
        {
            //Get the Settings
            var actual = BuildSetting();

            //Assertion
            Assert.That(actual.GetColumnType(0), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(1), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(2), Is.EqualTo(ColumnType.Text)); //By Default a column is Text
            Assert.That(actual.GetColumnType(3), Is.EqualTo(ColumnType.Text));
            Assert.That(actual.GetColumnType(4), Is.EqualTo(ColumnType.Numeric)); 
            Assert.That(actual.GetColumnType(8), Is.EqualTo(ColumnType.Text));
            //default for a SettingsValue is Numeric
            Assert.That(actual.GetColumnType(9), Is.EqualTo(ColumnType.Numeric));

        }
        
        [Test]
        public void GetTolerance_EqualToAndColumnsDefinedCorrectResult()
        {
            //Get the Settings
            var actual = BuildSetting();

            //Assertion
            //apply specific value
            Assert.That(actual.GetTolerance(1).ValueString, Is.EqualTo("1"));
            //apply default value
            Assert.That(actual.GetTolerance(2), Is.Null); //We haven't a Numeric column
            Assert.That(actual.GetTolerance(4).ValueString, Is.EqualTo("100"));
            Assert.That(actual.GetTolerance(9).ValueString, Is.EqualTo("100"));
        }
        
        [Test]
        public void IsKey_EqualToAndColumnsDefinedCorrectResult()
        {
            //Get the Settings
            var actual = BuildSetting();

            //Assertion
            Assert.That(actual.GetColumnRole(0), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(1), Is.Not.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(2), Is.Not.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(3), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(4), Is.Not.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(5), Is.Not.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(8), Is.EqualTo(ColumnRole.Key));
            Assert.That(actual.GetColumnRole(9), Is.Not.EqualTo(ColumnRole.Key));
        }
        
        [Test]
        public void IsValue_EqualToAndColumnsDefinedCorrectResult()
        {
            //Get the Settings
            var actual = BuildSetting();

            //Assertion
            Assert.That(actual.GetColumnRole(0), Is.Not.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(1), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(2), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(3), Is.Not.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(4), Is.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(5), Is.Not.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(8), Is.Not.EqualTo(ColumnRole.Value));
            Assert.That(actual.GetColumnRole(9), Is.EqualTo(ColumnRole.Value));
        }
        
        [Test]
        public void IsNumeric_EqualToAndColumnsDefinedCorrectResult()
        {
            //Get the Settings
            var actual = BuildSetting();

            //Assertion
            Assert.That(actual.GetColumnType(0), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(1), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(2), Is.Not.EqualTo(ColumnType.Numeric)); //By Default a Column is Textual
            Assert.That(actual.GetColumnType(3), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(4), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(8), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(9), Is.EqualTo(ColumnType.Numeric));
        }
        
        [Test]
        public void IsNumeric_EqualToAndColumnsDefinedWithValuesDefaultTypeCorrectResult()
        {
            //Get the Settings
            var xml = BuildEqualToXmlIndex();
            xml.ValuesDefaultType = ColumnType.DateTime;
            //get settings
            var actual = (xml.GetSettings() as SettingsResultSetComparisonByIndex);
            //Set the columnCount
            actual .ApplyTo(10);

            //Assertion
            Assert.That(actual.GetColumnType(0), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(1), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(2), Is.Not.EqualTo(ColumnType.Numeric)); //By Default a Key column is Textual
            Assert.That(actual.GetColumnType(2), Is.Not.EqualTo(ColumnType.DateTime)); //By Default a Key column is Textual
            Assert.That(actual.GetColumnType(2), Is.Not.EqualTo(ColumnType.Boolean)); //By Default a Key column is Textual
            Assert.That(actual.GetColumnType(3), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(4), Is.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(8), Is.Not.EqualTo(ColumnType.Numeric));
            Assert.That(actual.GetColumnType(9), Is.Not.EqualTo(ColumnType.Numeric)); //The default type for a Value column is dateTime
            Assert.That(actual.GetColumnType(9), Is.EqualTo(ColumnType.DateTime)); //The default type for a Value column is dateTime
        }
        
        private EqualToXml BuildEqualToXmlIndex()
        {
            //Buiding object used during test
            var xml = new EqualToXml();
            //default values/def
            xml.KeysDef = SettingsResultSetComparisonByIndex.KeysChoice.AllExpectLast;
            //default values/def
            xml.ValuesDefaultType = ColumnType.Numeric;
            //default tolerance
            xml.Tolerance = "100";

            //Build a value column (numeric, specific tolerance)
            var colXml = new ColumnDefinitionXml();
            colXml.Index = 1;
            colXml.Role = ColumnRole.Value;
            colXml.Type = ColumnType.Numeric;
            colXml.Tolerance = "1";

            //Build a value column (without info)
            var colLightXml = new ColumnDefinitionXml();
            colLightXml.Index = 2;
            colLightXml.Role = ColumnRole.Value;

            //Build a value column (numeric)
            var col4Xml = new ColumnDefinitionXml();
            col4Xml.Index = 4;
            col4Xml.Role = ColumnRole.Value;
            col4Xml.Type = ColumnType.Numeric;

            //Build a ignore column (without info)
            var colIgnoreXml = new ColumnDefinitionXml();
            colIgnoreXml.Index = 5;
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
        
        private SettingsResultSetComparisonByIndex BuildSetting()
        {
            var xml = BuildEqualToXmlIndex();
            //get settings
            var settings = (xml.GetSettings() as SettingsResultSetComparisonByIndex);

            //Set the columnCount
            settings.ApplyTo(10);

            return settings;
        }
        

    }
}
