using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage.Helper;

namespace NBi.Framework.FailureMessage
{
    public class DataRowsMessage : SampledFailureMessage<DataRow>
    {
        public DataRowsMessage(IFailureReportProfile profile)
            : base (profile)
        { }

        public void Build(IEnumerable<DataRow> expectedRows, IEnumerable<DataRow> actualRows, ResultSetCompareResult compareResult)
        {
            compareResult = compareResult ?? ResultSetCompareResult.Build(new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>());
            
            expected = BuildTable(expectedRows, Profile.ExpectedSet);
            actual = BuildTable(actualRows, Profile.ActualSet);
            compared = BuildNonEmptyTable(compareResult.Unexpected.Rows, "Unexpected", Profile.AnalysisSet);
            compared.Append(BuildNonEmptyTable(compareResult.Missing.Rows ?? new List<DataRow>(), "Missing", Profile.AnalysisSet));
            compared.Append(BuildNonEmptyTable(compareResult.Duplicated.Rows ?? new List<DataRow>(), "Duplicated", Profile.AnalysisSet));
            compared.Append(BuildCompareTable(compareResult.NonMatchingValue.Rows ?? new List<DataRow>(), "Non matching value", Profile.AnalysisSet));
        }

        private MarkdownContainer BuildTable(IEnumerable<DataRow> rows, FailureReportSetType sampling)
        {
            var tableBuilder = new TableHelper();
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

            if (IsSampled(rows))
            {
                var rowsSkipped = string.Format("{0} (of {1}) rows have been skipped for display purpose.", CountExcludedRows(rows), rows.Count());
                container.Append(rowsSkipped.ToMarkdownParagraph());
            }

            return container;
        }

        private MarkdownContainer BuildNonEmptyTable(IEnumerable<DataRow> rows, string title, FailureReportSetType sampling)
        {
            var tableBuilder = new TableHelper();
            if (rows.Count() > 0)
                return BuildTable(tableBuilder, rows, title, sampling);
            else
                return new MarkdownContainer();
        }

        private MarkdownContainer BuildCompareTable(IEnumerable<DataRow> rows, string title, FailureReportSetType sampling)
        {
            var tableBuilder = new CompareTableHelper();
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
