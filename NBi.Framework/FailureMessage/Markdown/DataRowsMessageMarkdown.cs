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
using NBi.Extensibility;

namespace NBi.Framework.FailureMessage.Markdown
{
    class DataRowsMessageMarkdown : IDataRowsMessageFormatter
    {
        private readonly IDictionary<string, ISampler<IResultRow>> samplers;
        private readonly EngineStyle style;

        private MarkdownContainer? expected;
        private MarkdownContainer? actual;
        private MarkdownContainer? analysis;

        public DataRowsMessageMarkdown(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
        {
            this.style = style;
            this.samplers = samplers;
        }

        public void BuildComparaison(IEnumerable<IResultRow> expectedRows, IEnumerable<IResultRow> actualRows, ResultResultSet? compareResult)
        {
            compareResult ??= ResultResultSet.Build([], [], [], [], []);

            expected = BuildTable(style, expectedRows, samplers["expected"]);
            actual = BuildTable(style, actualRows, samplers["actual"]);
            analysis = BuildNonEmptyTable(style, compareResult.Unexpected ?? [], "Unexpected", samplers["analysis"]);
            analysis.Append(BuildNonEmptyTable(style, compareResult.Missing ?? [], "Missing", samplers["analysis"]));
            analysis.Append(BuildNonEmptyTable(style, compareResult.Duplicated ?? [], "Duplicated", samplers["analysis"]));
            analysis.Append(BuildCompareTable(style, compareResult.NonMatchingValue?.Rows ?? [], "Non matching value", samplers["analysis"]));
        }

        public void BuildDuplication(IEnumerable<IResultRow> actualRows, ResultUniqueRows result)
        {
            actual = new MarkdownContainer();
            var sb = new StringBuilder();
            var uniqueCount = actualRows.Count() - result.Rows?.Sum(x => Convert.ToInt32(x[0])) ?? 0;
            sb.Append($"The actual result-set has {result.RowCount} rows.");
            sb.Append($" {uniqueCount} row{(uniqueCount > 1 ? "s are" : " is")} effectively unique");
            sb.Append($" and {result.Values.Count()} distinct row{(result.Values.Count() > 1 ? "s are" : " is")} duplicated.");
            actual.Append(new Paragraph(sb.ToString()));
            actual.Append(BuildTable(style, actualRows, samplers["actual"]));
            analysis = new MarkdownContainer();
            analysis.Append(BuildNonEmptyTable(style, result.Rows!, "Duplicated", samplers["analysis"]));
        }

        public void BuildFilter(IEnumerable<IResultRow> actualRows, IEnumerable<IResultRow> filteredRows)
        {
            actual = BuildTable(style, actualRows, samplers["actual"]);
            analysis = BuildTable(style, filteredRows, samplers["actual"]);
        }
        public void BuildCount(IEnumerable<IResultRow> actualRows)
        {
            actual = BuildTable(style, actualRows, samplers["actual"]);
        }

        private MarkdownContainer BuildTable(EngineStyle style, IEnumerable<IResultRow> rows, ISampler<IResultRow> sampler)
        {
            var tableBuilder = new TableHelperMarkdown(style);
            return BuildTable(tableBuilder, rows, string.Empty, sampler);
        }

        private MarkdownContainer BuildTable(TableHelperMarkdown tableBuilder, IEnumerable<IResultRow> rows, string title, ISampler<IResultRow> sampler)
        {
            rows = rows ?? new List<IResultRow>();

            sampler.Build(rows);
            var table = tableBuilder.Build(sampler.GetResult());

            var container = new MarkdownContainer();

            if (!string.IsNullOrEmpty(title))
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

        private MarkdownContainer BuildNonEmptyTable(EngineStyle style, IEnumerable<IResultRow> rows, string title, ISampler<IResultRow> sampler)
        {
            var tableBuilder = new TableHelperMarkdown(style);
            if (rows !=null && rows.Count() > 0)
                return BuildTable(tableBuilder, rows, title, sampler);
            else
                return new MarkdownContainer();
        }


        private MarkdownContainer BuildCompareTable(EngineStyle style, IEnumerable<IResultRow> rows, string title, ISampler<IResultRow> sampler)
        {
            var tableBuilder = new CompareTableHelperMarkdown(style);
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
            if (samplers["expected"] is NoneSampler<IResultRow>)
                return "Display skipped.";
            else
                return expected?.ToMarkdown() ?? string.Empty;
        }

        public string RenderActual()
        {
            if (samplers["actual"] is NoneSampler<IResultRow>)
                return "Display skipped.";
            else
                return actual?.ToMarkdown() ?? string.Empty;
        }

        public string RenderAnalysis()
        {
            if (samplers["analysis"] is NoneSampler<IResultRow>)
                return "Display skipped.";
            else
                return analysis?.ToMarkdown() ?? string.Empty;
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
