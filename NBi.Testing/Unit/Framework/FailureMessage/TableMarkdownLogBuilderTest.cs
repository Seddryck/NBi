using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NBi.Framework.FailureMessage;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;

namespace NBi.Testing.Unit.Framework.FailureMessage
{
    public class TableMarkdownLogBuilderTest
    {
        [Test]
        public void Build_TwoRows_4Lines()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var msg = new TableMarkdownLogBuilder();
            var value = msg.Build(dataTable.Rows.Cast<DataRow>()).ToMarkdown();

            Assert.That(value.Count<char>(c => c == '\n'), Is.EqualTo(4));

            var secondLineIndex = value.IndexOf('\n');
            var thirdLineIndex = value.IndexOf('\n', secondLineIndex + 1);
            var secondLine = value.Substring(secondLineIndex+1, thirdLineIndex-secondLineIndex-2);
            Assert.That(secondLine.Distinct<char>().Count(), Is.EqualTo(3));
            Assert.That(secondLine.Distinct<char>(), Has.Member(' '));
            Assert.That(secondLine.Distinct<char>(), Has.Member('-'));
            Assert.That(secondLine.Distinct<char>(), Has.Member('|'));
        }

        [Test]
        public void Build_TwoRows_SeperationLineCorrectlyWritten()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var msg = new TableMarkdownLogBuilder();
            var value = msg.Build(dataTable.Rows.Cast<DataRow>()).ToMarkdown();

            var secondLineIndex = value.IndexOf('\n');
            var thirdLineIndex = value.IndexOf('\n', secondLineIndex + 1);
            var secondLine = value.Substring(secondLineIndex + 1, thirdLineIndex - secondLineIndex - 2);
            Assert.That(secondLine.Distinct<char>().Count(), Is.EqualTo(3));
            Assert.That(secondLine.Distinct<char>(), Has.Member(' '));
            Assert.That(secondLine.Distinct<char>(), Has.Member('-'));
            Assert.That(secondLine.Distinct<char>(), Has.Member('|'));
        }

        [Test]
        public void Build_TwoRows_ColumnDelimitersAlligned()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var msg = new TableMarkdownLogBuilder();
            var value = msg.Build(dataTable.Rows.Cast<DataRow>()).ToMarkdown();
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
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            var numericDataColumn = new DataColumn("Numeric value");
            numericDataColumn.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            dataTable.Columns.Add(numericDataColumn);
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10.752, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20.8445585, false }, false);

            var msg = new TableMarkdownLogBuilder();
            var value = msg.Build(dataTable.Rows.Cast<DataRow>()).ToMarkdown();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            Assert.That(value, Is.StringContaining("10.752 "));
            Assert.That(value, Is.StringContaining("20.8445585"));
        }

        
    }
}
