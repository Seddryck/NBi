using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MarkdownLog;
using NBi.Core.ResultSet;

namespace NBi.Framework.FailureMessage
{
    public class EqualToMessage : IFailureMessage
    {
        protected readonly int maxRowCount;
        protected readonly int sampleRowCount;

        protected MarkdownContainer expected;
        protected MarkdownContainer actual;
        protected MarkdownContainer compared;

        public EqualToMessage() : this(10,15)
        { }

        public EqualToMessage(int sampleRowCount, int maxRowCount)
        {
            this.sampleRowCount = sampleRowCount;
            this.maxRowCount = maxRowCount;
        }

        public void Build(IEnumerable<DataRow> expectedRows, IEnumerable<DataRow> actualRows, ResultSetCompareResult compareResult)
        {
            compareResult = compareResult ?? ResultSetCompareResult.Build(new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>(), new List<DataRow>());
            
            expected = BuildTable(expectedRows);
            actual = BuildTable(expectedRows);
            compared = BuildNonEmptyTable(compareResult.Unexpected.Rows, "Unexpected");
            compared.Append(BuildNonEmptyTable(compareResult.Missing.Rows ?? new List<DataRow>(), "Missing"));
            compared.Append(BuildNonEmptyTable(compareResult.Duplicated.Rows ?? new List<DataRow>(), "Duplicated"));
            compared.Append(BuildNonEmptyTable(compareResult.NonMatchingValue.Rows ?? new List<DataRow>(), "Non matching value"));
        }

        private MarkdownContainer BuildTable(IEnumerable<DataRow> rows)
        {
            return BuildTable(rows, string.Empty);
        }

        private MarkdownContainer BuildTable(IEnumerable<DataRow> rows, string title)
        {
            rows = rows ?? new List<DataRow>();
            
            var tableBuilder = new TableMarkdownLogBuilder();
            var table = tableBuilder.Build(rows.Take(rows.Count() > maxRowCount ? sampleRowCount : rows.Count()));

            var container = new MarkdownContainer();

            if (!String.IsNullOrEmpty(title))
            {
                var titleText = string.Format("{0} rows:", title);
                container.Append(titleText.ToMarkdownSubHeader());
            }

            container.Append(BuildRowCount(rows.Count()));
            container.Append(table);

            if (rows.Count() > maxRowCount)
            {
                var rowsSkipped = string.Format("{0} (of {1}) rows have been skipped for display purpose.", rows.Count() - sampleRowCount, rows.Count());
                container.Append(rowsSkipped.ToMarkdownParagraph());
            }

            return container;
        }

        private MarkdownContainer BuildNonEmptyTable(IEnumerable<DataRow> rows, string title)
        {
            var tableBuilder = new TableMarkdownLogBuilder();
            if (rows.Count() > 0)
                return BuildTable(rows, title);
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

        public string RenderExpected()
        {
            return expected.ToMarkdown();
        }

        public string RenderActual()
        {
            return actual.ToMarkdown();
        }

        public string RenderCompared()
        {
            return compared.ToMarkdown();
        }
    }
}
