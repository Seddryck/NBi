using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Uniqueness;
using NBi.Framework.FailureMessage.Helper;

namespace NBi.Framework.FailureMessage
{
    public class DataRowsMessage : SampledFailureMessage<DataRow>
    {
        private readonly ComparisonStyle style;
        public DataRowsMessage(ComparisonStyle style, IFailureReportProfile profile)
            : base (profile)
        {
            this.style = style;
        }

        public void BuildComparaison(IEnumerable<DataRow> expectedRows, IEnumerable<DataRow> actualRows, ResultSetCompareResult compareResult)
        {
            compareResult = compareResult ?? ResultSetCompareResult.Build(new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>());
            
            expected = BuildTable(style, expectedRows, Profile.ExpectedSet);
            actual = BuildTable(style, actualRows, Profile.ActualSet);
            compared = BuildNonEmptyTable(style, compareResult.Unexpected, "Unexpected", Profile.AnalysisSet);
            compared.Append(BuildNonEmptyTable(style, compareResult.Missing ?? new List<DataRow>(), "Missing", Profile.AnalysisSet));
            compared.Append(BuildNonEmptyTable(style, compareResult.Duplicated ?? new List<DataRow>(), "Duplicated", Profile.AnalysisSet));
            compared.Append(BuildCompareTable(style, compareResult.NonMatchingValue.Rows ?? new List<DataRow>(), "Non matching value", Profile.AnalysisSet));
        }

        public void BuildDuplication(IEnumerable<DataRow> actualRows, UniqueRowsResult result)
        {
            actual = new MarkdownContainer();
            actual.Append(new Paragraph($"The actual result-set has {result.RowCount} rows and contains {result.Values.Count()} unique rows duplicated."));
            actual.Append(BuildTable(style, actualRows, Profile.ActualSet));
            duplicated = new MarkdownContainer();
            duplicated.Append(BuildNonEmptyTable(style, result.Rows, "Duplicated", Profile.AnalysisSet));
        }

        public void BuildFilter(IEnumerable<DataRow> actualRows, IEnumerable<DataRow> filteredRows)
        {
            actual = BuildTable(style, actualRows, Profile.ActualSet);
            filtered = BuildTable(style, filteredRows, Profile.ActualSet);
        }
        public void BuildCount(IEnumerable<DataRow> actualRows)
        {
            actual = BuildTable(style, actualRows, Profile.ActualSet);
        }

        private MarkdownContainer BuildTable(ComparisonStyle style, IEnumerable<DataRow> rows, FailureReportSetType sampling)
        {
            var tableBuilder = new TableHelper(style);
            return BuildTable(tableBuilder, rows, string.Empty, sampling);
        }

        private MarkdownContainer BuildTable(TableHelper tableBuilder, IEnumerable<DataRow> rows, string title, FailureReportSetType sampling)
        {
            rows = rows ?? new List<DataRow>();

            var table = tableBuilder.Build(Sample(rows, sampling));

            var container = new MarkdownContainer();

            if (!String.IsNullOrEmpty(title))
            {
                var titleText = string.Format("{0} rows:", title);
                container.Append(titleText.ToMarkdownSubHeader());
            }

            container.Append(BuildRowCount(rows.Count()));
            container.Append(table);

            if (IsSampled(rows, sampling))
            {
                var rowsSkipped = string.Format("{0} (of {1}) rows have been skipped for display purpose.", CountExcludedRows(rows), rows.Count());
                container.Append(rowsSkipped.ToMarkdownParagraph());
            }

            return container;
        }

        private MarkdownContainer BuildNonEmptyTable(ComparisonStyle style, IEnumerable<DataRow> rows, string title, FailureReportSetType sampling)
        {
            var tableBuilder = new TableHelper(style);
            if (rows.Count() > 0)
                return BuildTable(tableBuilder, rows, title, sampling);
            else
                return new MarkdownContainer();
        }


        private MarkdownContainer BuildCompareTable(ComparisonStyle style, IEnumerable<DataRow> rows, string title, FailureReportSetType sampling)
        {
            var tableBuilder = new CompareTableHelper(style);
            if (rows.Count() > 0)
                return BuildTable(tableBuilder, rows, title, sampling);
            else
                return new MarkdownContainer();
        }

        protected Paragraph BuildRowCount(int rowCount)
        {
            var sbInfo = new StringBuilder();
            sbInfo.AppendFormat("ResultSet with {0} row", rowCount);
            if (rowCount > 1)
                sbInfo.Append('s');
            return (sbInfo.ToString().ToMarkdownParagraph());
        }

    }
}
