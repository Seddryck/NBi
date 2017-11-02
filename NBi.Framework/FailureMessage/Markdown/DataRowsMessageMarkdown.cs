using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage.Markdown.Helper;
using NBi.Framework.Sampling;
using NBi.Core.ResultSet.Uniqueness;

namespace NBi.Framework.FailureMessage.Markdown
{
    class DataRowsMessageMarkdown : IDataRowsMessageFormatter
    {
        private readonly IDictionary<string, ISampler<DataRow>> samplers;
        private readonly ComparisonStyle style;

        private MarkdownContainer expected;
        private MarkdownContainer actual;
        private MarkdownContainer analysis;

        public DataRowsMessageMarkdown(ComparisonStyle style, IDictionary<string, ISampler<DataRow>> samplers)
        {
            this.style = style;
            this.samplers = samplers;
        }

        public void BuildComparaison(IEnumerable<DataRow> expectedRows, IEnumerable<DataRow> actualRows, ResultSetCompareResult compareResult)
        {
            compareResult = compareResult ?? ResultSetCompareResult.Build(new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>());

            expected = BuildTable(style, expectedRows, samplers["expected"]);
            actual = BuildTable(style, actualRows, samplers["actual"]);
            analysis = BuildNonEmptyTable(style, compareResult.Unexpected, "Unexpected", samplers["analysis"]);
            analysis.Append(BuildNonEmptyTable(style, compareResult.Missing ?? new List<DataRow>(), "Missing", samplers["analysis"]));
            analysis.Append(BuildNonEmptyTable(style, compareResult.Duplicated ?? new List<DataRow>(), "Duplicated", samplers["analysis"]));
            analysis.Append(BuildCompareTable(style, compareResult.NonMatchingValue.Rows ?? new List<DataRow>(), "Non matching value", samplers["analysis"]));
        }

        public void BuildDuplication(IEnumerable<DataRow> actualRows, UniqueRowsResult result)
        {
            actual = new MarkdownContainer();
            var sb = new StringBuilder();
            var uniqueCount = actualRows.Count() - result.Rows.Sum(x => Convert.ToInt32(x[0]));
            sb.Append($"The actual result-set has {result.RowCount} rows.");
            sb.Append($" {uniqueCount} row{(uniqueCount > 1 ? "s are" : " is")} effectively unique");
            sb.Append($" and {result.Values.Count()} distinct row{(result.Values.Count() > 1 ? "s are" : " is")} duplicated.");
            actual.Append(new Paragraph(sb.ToString()));
            actual.Append(BuildTable(style, actualRows, samplers["actual"]));
            analysis = new MarkdownContainer();
            analysis.Append(BuildNonEmptyTable(style, result.Rows, "Duplicated", samplers["analysis"]));
        }

        public void BuildFilter(IEnumerable<DataRow> actualRows, IEnumerable<DataRow> filteredRows)
        {
            actual = BuildTable(style, actualRows, samplers["actual"]);
            analysis = BuildTable(style, filteredRows, samplers["actual"]);
        }
        public void BuildCount(IEnumerable<DataRow> actualRows)
        {
            actual = BuildTable(style, actualRows, samplers["actual"]);
        }

        private MarkdownContainer BuildTable(ComparisonStyle style, IEnumerable<DataRow> rows, ISampler<DataRow> sampler)
        {
            var tableBuilder = new TableHelper(style);
            return BuildTable(tableBuilder, rows, string.Empty, sampler);
        }

        private MarkdownContainer BuildTable(TableHelper tableBuilder, IEnumerable<DataRow> rows, string title, ISampler<DataRow> sampler)
        {
            rows = rows ?? new List<DataRow>();

            sampler.Build(rows);
            var table = tableBuilder.Build(sampler.GetResult());

            var container = new MarkdownContainer();

            if (!String.IsNullOrEmpty(title))
            {
                var titleText = string.Format($"{title} rows:");
                container.Append(titleText.ToMarkdownSubHeader());
            }

            container.Append(BuildRowCount(rows.Count()));
            container.Append(table);

            if (sampler.GetIsSampled())
            {
                var rowsSkipped = string.Format($"{sampler.GetExcludedRowCount()} (of {rows.Count()}) rows have been skipped for display purpose.");
                container.Append(rowsSkipped.ToMarkdownParagraph());
            }

            return container;
        }

        private MarkdownContainer BuildNonEmptyTable(ComparisonStyle style, IEnumerable<DataRow> rows, string title, ISampler<DataRow> sampler)
        {
            var tableBuilder = new TableHelper(style);
            if (rows.Count() > 0)
                return BuildTable(tableBuilder, rows, title, sampler);
            else
                return new MarkdownContainer();
        }


        private MarkdownContainer BuildCompareTable(ComparisonStyle style, IEnumerable<DataRow> rows, string title, ISampler<DataRow> sampler)
        {
            var tableBuilder = new CompareTableHelper(style);
            if (rows.Count() > 0)
                return BuildTable(tableBuilder, rows, title, sampler);
            else
                return new MarkdownContainer();
        }

        protected Paragraph BuildRowCount(int rowCount)
        {
            return ($"Result-set with {rowCount} row{(rowCount > 1 ? "s" : string.Empty)}".ToMarkdownParagraph());
        }

        public string RenderExpected()
        {
            if (samplers["expected"] is NoneSampler<DataRow>)
                return "Display skipped.";
            else
                return expected.ToMarkdown();
        }

        public string RenderActual()
        {
            if (samplers["actual"] is NoneSampler<DataRow>)
                return "Display skipped.";
            else
                return actual.ToMarkdown();
        }

        public string RenderAnalysis()
        {
            if (samplers["analysis"] is NoneSampler<DataRow>)
                return "Display skipped.";
            else
                return analysis.ToMarkdown();
        }

        public string RenderMessage()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Execution of the query doesn't match the expected result");
            sb.AppendLine();
            sb.AppendLine(RenderExpected());
            sb.AppendLine();
            sb.AppendLine(RenderActual());
            sb.AppendLine();
            sb.AppendLine(RenderAnalysis());
            return sb.ToString();
        }

    }
}
