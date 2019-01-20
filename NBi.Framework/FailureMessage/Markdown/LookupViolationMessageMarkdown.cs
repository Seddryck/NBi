using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Framework.FailureMessage.Common;
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
    class LookupViolationMessageMarkdown : LookupViolationMessage<MarkdownContainer>
    {

        public LookupViolationMessageMarkdown(IDictionary<string, ISampler<DataRow>> samplers)
            : base(samplers) { }

        protected override MarkdownContainer RenderStandardTable(IEnumerable<DataRow> rows, IEnumerable<ColumnMetadata> metadata, ISampler<DataRow> sampler, string title)
        {
            sampler.Build(rows);
            var tableHelper = new StandardTableHelper(sampler.GetResult(), metadata);

            var table = tableHelper.Render();
            return RenderTextAroundTable(table, rows, title, sampler);
        }

        private MarkdownContainer RenderTextAroundTable(MarkdownContainer table, IEnumerable<DataRow> rows, string title, ISampler<DataRow> sampler)
        {
            rows = rows ?? new List<DataRow>();

            var container = new MarkdownContainer();

            if (!string.IsNullOrEmpty(title))
            {
                var titleText = string.Format($"{title} rows:");
                container.Append(titleText.ToMarkdownSubHeader());
            }

            container.Append(BuildRowCount(rows.Count()));
            container.Append(table);

            if (sampler?.GetIsSampled() ?? false)
            {
                var rowsSkipped = string.Format($"{sampler.GetExcludedRowCount()} (of {rows.Count()}) rows have been skipped for display purpose.");
                container.Append(rowsSkipped.ToMarkdownParagraph());
            }

            return container;
        }

        protected override MarkdownContainer RenderAnalysis(LookupViolationCollection violations, IEnumerable<ColumnMetadata> metadata, ISampler<DataRow> sampler, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings)
        {
            var container = new MarkdownContainer();
            container.Append("Analysis".ToMarkdownHeader());

            foreach (var state in violations.Values.Select(x => x.State).Distinct())
            {
                container.Append(GetExplanationText(violations, state).ToMarkdownParagraph());
                ITableHelper tableHelper = null;
                    
                if (state == RowViolationState.Mismatch)
                {
                    tableHelper = new LookupTableHelper(
                            violations.Values.Where(x => x is LookupMatchesViolationInformation)
                            .Cast<LookupMatchesViolationInformation>()
                            .SelectMany(x => x.CandidateRows)
                        , metadata);
                }
                else
                {
                    var rows = violations.Values.Where(x => x is LookupExistsViolationInformation)
                            .Cast<LookupExistsViolationInformation>()
                            .SelectMany(x => x.CandidateRows);
                    sampler.Build(rows);
                    tableHelper = new StandardTableHelper(sampler.GetResult(), metadata);
                }
                var table = tableHelper.Render();
                container.Append(RenderTextAroundTable(table, violations.GetRows(state), Textify(state), state == RowViolationState.Mismatch ? null : sampler));
            }
            return container;
        }

        protected virtual IEnumerable<IColumnDefinition> BuildMetadata(ColumnMappingCollection mappings, ColumnRole role, Func<ColumnMapping, IColumnIdentifier> identify)
        {
            foreach (var mapping in mappings ?? new ColumnMappingCollection())
                yield return new Column()
                {
                    Identifier = identify.Invoke(mapping),
                    Role = role,
                    Type = mapping.Type,
                };
        }

        private Paragraph BuildRowCount(int rowCount)
            => ($"Result-set with {rowCount} row{(rowCount > 1 ? "s" : string.Empty)}".ToMarkdownParagraph());

        public override string RenderReference() => reference.ToMarkdown();
        public override string RenderCandidate() => candidate.ToMarkdown();
        public override string RenderAnalysis() => analysis.ToMarkdown();

        private string Textify(RowViolationState s)
        {
            switch (s)
            {
                case RowViolationState.Missing: return "Missing";
                case RowViolationState.Unexpected: return "Unexpected";
                case RowViolationState.Mismatch: return "Non-matching";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private string GetExplanationText(LookupViolationCollection violations, RowViolationState state)
        {
            string Pluralize(int x) => x > 1 ? "s" : string.Empty;
            string Verbalize(int x) => x > 1 ? "are" : "is";
            string PluralizeVerb(int x) => x > 1 ? string.Empty : "s";
            string This(int x) => x > 1 ? $"These {x} distinct" : $"This";
            string Textify(RowViolationState s, int x)
            {
                switch (s)
                {
                    case RowViolationState.Missing: return $"missing. It means {This(x).ToLower()} key{Pluralize(x)} {Verbalize(x)} not available in the system-under-test but {Verbalize(x)} found in the result-set defined in the assertion";
                    case RowViolationState.Unexpected: return $"unexpected. It means {This(x).ToLower()} key{Pluralize(x)} {Verbalize(x)} available in the system-under-test but {Verbalize(x)} not found in the result-set defined in the assertion";
                    case RowViolationState.Mismatch: return $"non-matching. It means the values associated to {This(x).ToLower()} key{Pluralize(x)} {Verbalize(x)} not equal in the candidate and reference tables";
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            string GetText(int x, int y) => $"{x} distinct key{Pluralize(x)} found in the candidate table {Verbalize(x)} {Textify(state, x)}. {This(x)} key{Pluralize(x)} appear{PluralizeVerb(x)} in {y} row{Pluralize(y)} of the candidate table.";

            var count = violations.Where(x => x.Value.State == state).Count();
            var countRow = violations.Where(x => x.Value.State == state).Sum(x => x.Value.Rows.Count());
            return GetText(count, countRow);
        }
        
    }
}
