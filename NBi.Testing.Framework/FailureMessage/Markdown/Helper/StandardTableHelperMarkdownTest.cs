using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage.Markdown.Helper;
using MarkdownLog;
using NBi.Framework.Sampling;
using System.Text.RegularExpressions;
using NBi.Extensibility;

namespace NBi.Framework.Testing.FailureMessage.Markdown.Helper
{
    public class StandardTableHelperMarkdownTest
    {
        [Test]
        public void Build_TwoRows_SevenLines()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            dataTable.LoadDataRow(new object[] { "Beta", 20, false }, false);
            var rs = new DataTableResultSet(dataTable);

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Id"), Role = ColumnRole.Key };

            var sampler = new FullSampler<IResultRow>();
            sampler.Build(rs.Rows);
            var msg = new StandardTableHelperMarkdown(rs.Rows
                , new ColumnMetadata[] { idDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();

            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(7));

            var indexes = value.IndexOfAll('\n').ToArray();
            var dashLine = value.Substring(indexes[3] + 1, indexes[4] - indexes[3] - 2);
            Assert.That(dashLine.Distinct().Count(), Is.EqualTo(3));
            Assert.That(dashLine.Distinct(), Has.Member(' '));
            Assert.That(dashLine.Distinct(), Has.Member('-'));
            Assert.That(dashLine.Distinct(), Has.Member('|'));
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
            var rs = new DataTableResultSet(dataTable);

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#0"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#1"), Role = ColumnRole.Value };
            var booleanDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#2"), Role = ColumnRole.Value };

            var sampler = new FullSampler<IResultRow>();
            sampler.Build(rs.Rows);
            var msg = new StandardTableHelperMarkdown(rs.Rows
                , new ColumnMetadata[] { idDefinition, numericDefinition, booleanDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();

            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(7));

            var indexes = value.IndexOfAll('\n').ToArray();

            var titleLine = value.Substring(indexes[1] + 1, indexes[2] - indexes[1] - 2);
            Assert.That(titleLine.Replace(" ", ""), Is.EqualTo("#0(Id)|#1(Numericvalue)|#2(Booleanvalue)"));
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

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#0"), Role = ColumnRole.Key };

            var sampler = new FullSampler<IResultRow>();
            sampler.Build(rs.Rows);
            var msg = new StandardTableHelperMarkdown(rs.Rows
                , new ColumnMetadata[] { idDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();

            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(7));

            var indexes = value.IndexOfAll('\n').ToArray();

            var thirdLine = value.Substring(indexes[1] + 1, indexes[2] - indexes[1] - 2);
            Assert.That(thirdLine.Replace(" ", ""), Is.EqualTo("#0(Id)|#1(Numericvalue)|#2(Booleanvalue)"));
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
            var rs = new DataTableResultSet(dataTable);

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#0"), Role = ColumnRole.Key };

            var sampler = new FullSampler<IResultRow>();
            sampler.Build(rs.Rows);
            var msg = new StandardTableHelperMarkdown(rs.Rows
                , new ColumnMetadata[] { idDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();

            var indexes = value.IndexOfAll('\n').ToArray();
            var dashLine = value.Substring(indexes[3] + 1, indexes[4] - indexes[3] - 2);
            Assert.That(dashLine.Distinct().Count(), Is.EqualTo(3));
            Assert.That(dashLine.Distinct(), Has.Member(' '));
            Assert.That(dashLine.Distinct(), Has.Member('-'));
            Assert.That(dashLine.Distinct(), Has.Member('|'));
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

            var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#0"), Role = ColumnRole.Key };

            var sampler = new FullSampler<IResultRow>();
            sampler.Build(rs.Rows);
            var msg = new StandardTableHelperMarkdown(rs.Rows
                , new ColumnMetadata[] { idDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            int pos = 0;
            while ((pos = lines[0].IndexOf('|', pos + 1)) > 0)
            {
                foreach (var line in lines.TakeWhile(l => l.Length > 0))
                    Assert.That(line[pos], Is.EqualTo('|')); //, "The line '{0}' was expecting to have a '|' at position {1} but it was a '{2}'", new object[] { line, pos, line[pos] });
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
            var rs = new DataTableResultSet(dataTable);

            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value, Type = ColumnType.Numeric };

            var sampler = new FullSampler<IResultRow>();
            sampler.Build(rs.Rows);
            var msg = new StandardTableHelperMarkdown(rs.Rows
                , new ColumnMetadata[] { numericDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            Assert.That(value, Does.Contain("10.752 "));
            Assert.That(value, Does.Contain("20.8445585"));
        }
    }
}
