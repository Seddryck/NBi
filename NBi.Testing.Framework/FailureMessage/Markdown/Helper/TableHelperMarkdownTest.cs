using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage.Markdown.Helper;

namespace NBi.Framework.Testing.FailureMessage.Markdown.Helper
{
    public class TableHelperMarkdownTest
    {
        [Test]
        public void Build_TwoRows_5Lines()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);
            var rs = new DataTableResultSet(dataTable);

            var msg = new TableHelperMarkdown(EngineStyle.ByIndex);
            var value = msg.Build(rs.Rows).ToMarkdown();

            Assert.That(value.Count<char>(c => c == '\n'), Is.EqualTo(5));

            var secondLineIndex = value.IndexOf('\n');
            var thirdLineIndex = value.IndexOf('\n', secondLineIndex + 1);
            var fourthLineIndex = value.IndexOf('\n', thirdLineIndex + 1);
            var thirdLine = value.Substring(thirdLineIndex+1, fourthLineIndex-thirdLineIndex-2);
            Assert.That(thirdLine.Distinct<char>().Count(), Is.EqualTo(3));
            Assert.That(thirdLine.Distinct<char>(), Has.Member(' '));
            Assert.That(thirdLine.Distinct<char>(), Has.Member('-'));
            Assert.That(thirdLine.Distinct<char>(), Has.Member('|'));
        }

        [Test]
        public void Build_TwoRowsByOrdinal_FirstRow()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);
            var rs = new DataTableResultSet(dataTable);

            var msg = new TableHelperMarkdown(EngineStyle.ByName);
            var value = msg.Build(rs.Rows).ToMarkdown();

            Assert.That(value.Count<char>(c => c == '\n'), Is.EqualTo(5));

            var secondLineIndex = value.IndexOf('\n');
            
            var firstLine = value.Substring(0, secondLineIndex - 1);
            Assert.That(firstLine.Replace(" ",""), Is.EqualTo("Id|Numericvalue|Booleanvalue"));
        }

        [Test]
        public void Build_TwoRowsByName_FirstRow()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);
            var rs = new DataTableResultSet(dataTable);

            var msg = new TableHelperMarkdown(EngineStyle.ByIndex);
            var value = msg.Build(rs.Rows).ToMarkdown();

            Assert.That(value.Count<char>(c => c == '\n'), Is.EqualTo(5));

            var secondLineIndex = value.IndexOf('\n');

            var firstLine = value.Substring(0, secondLineIndex - 1);
            Assert.That(firstLine.Replace(" ", ""), Is.EqualTo("#0(Id)|#1(Numericvalue)|#2(Booleanvalue)"));
        }

        [Test]
        public void Build_TwoRows_SeperationLineCorrectlyWritten()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns["Id"]!.ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);
            var rs = new DataTableResultSet(dataTable);

            var msg = new TableHelperMarkdown(EngineStyle.ByIndex);
            var value = msg.Build(rs.Rows).ToMarkdown();

            var secondLineIndex = value.IndexOf('\n');
            var thirdLineIndex = value.IndexOf('\n', secondLineIndex + 1);
            var fourthLineIndex = value.IndexOf('\n', thirdLineIndex + 1);
            var thirdLine = value.Substring(thirdLineIndex + 1, fourthLineIndex - thirdLineIndex - 2);
            Assert.That(thirdLine.Distinct<char>().Count(), Is.EqualTo(3));
            Assert.That(thirdLine.Distinct<char>(), Has.Member(' '));
            Assert.That(thirdLine.Distinct<char>(), Has.Member('-'));
            Assert.That(thirdLine.Distinct<char>(), Has.Member('|'));
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
            var rs = new DataTableResultSet(dataTable);

            var msg = new TableHelperMarkdown(EngineStyle.ByIndex);
            var value = msg.Build(rs.Rows).ToMarkdown();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            int pos = 0;
            while ((pos = lines[0].IndexOf('|', pos + 1)) > 0)
            {
                foreach (var line in lines.TakeWhile(l => l.Length>0))
                    Assert.That(line[pos], Is.EqualTo('|'), "The line '{0}' was expecting to have a '|' at position {1} but it was a '{2}'", new object[] {line, pos, line[pos]});
            }
        }

        [Test]
        public void Build_TwoRows_NumericValuesNonRounded()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            var numericDataColumn = new DataColumn("Numeric value");
            numericDataColumn.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            dataTable.Columns.Add(numericDataColumn);
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10.752, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20.8445585, false }, false);
            var rs = new DataTableResultSet(dataTable);

            var msg = new TableHelperMarkdown(EngineStyle.ByIndex);
            var value = msg.Build(rs.Rows).ToMarkdown();

            Assert.That(value, Does.Contain("10.752 "));
            Assert.That(value, Does.Contain("20.8445585"));
        }

        
    }
}
