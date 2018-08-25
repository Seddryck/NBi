using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Framework.FailureMessage.Markdown.Helper;
using NBi.Framework.Sampling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown
{
    class ReferenceViolationsMessageMarkdown : IReferenceViolationsMessageFormatter
    {
        private readonly IDictionary<string, ISampler<KeyCollection>> keysCollectionSamplers;
        private readonly IDictionary<string, ISampler<DataRow>> dataRowsSamplers;

        private MarkdownContainer parent;
        private MarkdownContainer child;
        private MarkdownContainer analysis;

        public ReferenceViolationsMessageMarkdown(IDictionary<string, ISampler<KeyCollection>> keyCollectionSamplers, IDictionary<string, ISampler<DataRow>> dataRowSamplers)
        {
            this.keysCollectionSamplers = keyCollectionSamplers;
            this.dataRowsSamplers = dataRowSamplers;
        }

        public void Generate(IEnumerable<DataRow> parentRows, IEnumerable<DataRow> childRows, LookupViolations violations)
        {
            parent = BuildTable(parentRows, dataRowsSamplers["expected"]);
            child = BuildTable(childRows, dataRowsSamplers["actual"]);
            analysis = BuildMultipleTable(violations, "Missing references", keysCollectionSamplers["analysis"], dataRowsSamplers["analysis"]);
        }

        private MarkdownContainer BuildTable(IEnumerable<DataRow> rows, ISampler<DataRow> sampler)
        {
            var tableBuilder = new TableHelper(EngineStyle.ByIndex);
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

        private MarkdownContainer BuildMultipleTable(LookupViolations violations, string title, ISampler<KeyCollection> keyCollectionSampler, ISampler<DataRow> dataRowSampler)
        {
            var tableBuilder = new TableHelper(EngineStyle.ByIndex);
            var container = new MarkdownContainer();

            keyCollectionSampler.Build(violations.Keys);
            container.Append($"{violations.Keys.Count} missing reference{(violations.Keys.Count>1 ? "s" : string.Empty)}".ToMarkdownHeader());
            if (keyCollectionSampler.GetIsSampled())
                container.Append($"{keyCollectionSampler.GetExcludedRowCount()} (of {violations.Keys.Count}) rows have been skipped for display purpose.".ToMarkdownParagraph());

            foreach (var keyCollection in keyCollectionSampler.GetResult())
            {
                var rows = violations[keyCollection];
                container.Append($"Following reference is missing ({rows.Count} occurence{(rows.Count > 1 ? "s" : string.Empty)}):".ToMarkdownParagraph());
                container.Append(new BulletedList(keyCollection.Members.Cast<string>()));

                dataRowSampler.Build(rows);
                var table = tableBuilder.Build(dataRowSampler.GetResult());

                container.Append(BuildRowCount(rows.Count()));
                container.Append(table);

                if (dataRowSampler.GetIsSampled())
                {
                    var rowsSkipped = $"{dataRowSampler.GetExcludedRowCount()} (of {rows.Count()}) rows have been skipped for display purpose.";
                    container.Append(rowsSkipped.ToMarkdownParagraph());
                }
            }
            return container;
        }

        protected Paragraph BuildRowCount(int rowCount)
        {
            return ($"Result-set with {rowCount} row{(rowCount > 1 ? "s" : string.Empty)}".ToMarkdownParagraph());
        }

        public virtual string RenderExpected() => keysCollectionSamplers["expected"] is NoneSampler<KeyCollection> ?  "Display skipped." : parent.ToMarkdown();
        public virtual string RenderActual() => keysCollectionSamplers["actual"] is NoneSampler<KeyCollection> ? "Display skipped." : child.ToMarkdown();
        public virtual string RenderAnalysis() => keysCollectionSamplers["analysis"] is NoneSampler<KeyCollection> ? "Display skipped." : analysis.ToMarkdown();
        public virtual string RenderPredicate() => "Some references are missing and violate referential integrity";

        public string RenderMessage()
        {
            var sb = new StringBuilder();
            sb.AppendLine(RenderPredicate());
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
