using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage.Markdown.Helper;

namespace NBi.Testing.Unit.Framework.FailureMessage.Markdown.Helper
{
    public class StandardTableHelperTest
    {
        [Test]
        public void Build_TwoRows_5Lines()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Id"), Role = ColumnRole.Key };

            var msg = new StandardTableHelper(dataTable.Rows.Cast<DataRow>(), new ColumnMetadata[] { idDefinition });
            var value = msg.Render().ToMarkdown();

            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(5));

            var secondLineIndex = value.IndexOf('\n');
            var thirdLineIndex = value.IndexOf('\n', secondLineIndex + 1);
            var fourthLineIndex = value.IndexOf('\n', thirdLineIndex + 1);
            var thirdLine = value.Substring(thirdLineIndex+1, fourthLineIndex-thirdLineIndex-2);
            Assert.That(thirdLine.Distinct().Count(), Is.EqualTo(3));
            Assert.That(thirdLine.Distinct(), Has.Member(' '));
            Assert.That(thirdLine.Distinct(), Has.Member('-'));
            Assert.That(thirdLine.Distinct(), Has.Member('|'));
        }

        [Test]
        public void Build_TwoRowsByOrdinal_FirstRow()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#0"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#1"), Role = ColumnRole.Value };
            var booleanDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#2"), Role = ColumnRole.Value };

            var msg = new StandardTableHelper(dataTable.Rows.Cast<DataRow>(), new ColumnMetadata[] { idDefinition, numericDefinition, booleanDefinition });
            var value = msg.Render().ToMarkdown();

            Assert.That(value.Count<char>(c => c == '\n'), Is.EqualTo(5));

            var secondLineIndex = value.IndexOf('\n');
            
            var firstLine = value.Substring(0, secondLineIndex - 1);
            Assert.That(firstLine.Replace(" ",""), Is.EqualTo("#0(Id)|#1(Numericvalue)|#2(Booleanvalue)"));
        }

        [Test]
        public void Build_TwoRowsByName_FirstRow()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"].ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#0"), Role = ColumnRole.Key };

            var msg = new StandardTableHelper(dataTable.Rows.Cast<DataRow>(), new ColumnMetadata[] { idDefinition });
            var value = msg.Render().ToMarkdown();

            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(5));

            var secondLineIndex = value.IndexOf('\n');

            var firstLine = value.Substring(0, secondLineIndex - 1);
            Assert.That(firstLine.Replace(" ", ""), Is.EqualTo("#0(Id)|#1(Numericvalue)|#2(Booleanvalue)"));
        }

        [Test]
        public void Build_TwoRows_SeperationLineCorrectlyWritten()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#0"), Role = ColumnRole.Key };

            var msg = new StandardTableHelper(dataTable.Rows.Cast<DataRow>(), new ColumnMetadata[] { idDefinition });
            var value = msg.Render().ToMarkdown();

            var secondLineIndex = value.IndexOf('\n');
            var thirdLineIndex = value.IndexOf('\n', secondLineIndex + 1);
            var fourthLineIndex = value.IndexOf('\n', thirdLineIndex + 1);
            var thirdLine = value.Substring(thirdLineIndex + 1, fourthLineIndex - thirdLineIndex - 2);
            Assert.That(thirdLine.Distinct().Count(), Is.EqualTo(3));
            Assert.That(thirdLine.Distinct(), Has.Member(' '));
            Assert.That(thirdLine.Distinct(), Has.Member('-'));
            Assert.That(thirdLine.Distinct(), Has.Member('|'));
        }

        [Test]
        public void Build_TwoRows_ColumnDelimitersAlligned()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#0"), Role = ColumnRole.Key };

            var msg = new StandardTableHelper(dataTable.Rows.Cast<DataRow>(), new ColumnMetadata[] { idDefinition });
            var value = msg.Render().ToMarkdown();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            int pos = 0;
            while ((pos = lines[0].IndexOf('|', pos + 1)) > 0)
            {
                foreach (var line in lines.TakeWhile(l => l.Length>0))
                    Assert.That(line[pos], Is.EqualTo('|'), "The line '{0}' was expecting to have a '|' at position {1} but it was a '{2}'", new object[] {line, pos, line[pos]});
            }
        }

        [Test]
        [SetCulture("en-us")]
        public void Build_TwoRows_NumericValuesNonRounded()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            var numericDataColumn = new DataColumn("Numeric value");
            numericDataColumn.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            dataTable.Columns.Add(numericDataColumn);
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10.752, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20.8445585, false }, false);

            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value, Type=ColumnType.Numeric };

            var msg = new StandardTableHelper(dataTable.Rows.Cast<DataRow>(), new ColumnMetadata[] { numericDefinition });
            var value = msg.Render().ToMarkdown();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            Assert.That(value, Is.StringContaining("10.752 "));
            Assert.That(value, Is.StringContaining("20.8445585"));
        }

        
    }
}
