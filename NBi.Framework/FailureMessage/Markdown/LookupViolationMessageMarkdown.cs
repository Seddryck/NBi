using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
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
    class LookupViolationMessageMarkdown : ILookupViolationsMessageFormatter
    {
        private readonly IDictionary<string, ISampler<KeyCollection>> keysCollectionSamplers;
        private readonly IDictionary<string, ISampler<DataRow>> dataRowsSamplers;

        private MarkdownContainer reference;
        private MarkdownContainer candidate;
        private MarkdownContainer analysis;

        public LookupViolationMessageMarkdown(IDictionary<string, ISampler<KeyCollection>> keyCollectionSamplers, IDictionary<string, ISampler<DataRow>> dataRowSamplers)
        {
            this.keysCollectionSamplers = keyCollectionSamplers;
            this.dataRowsSamplers = dataRowSamplers;
        }

        public void Generate(IEnumerable<DataRow> referenceRows, IEnumerable<DataRow> candidateRows, LookupViolationCollection violations, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings)
        {
            var metadata = BuildMetadata(keyMappings, ColumnRole.Key, x => x.ReferenceColumn)
                .Union(BuildMetadata(valueMappings, ColumnRole.Value, x => x.ReferenceColumn));
            reference = BuildTable(referenceRows, metadata, dataRowsSamplers["reference"], "Reference");

            metadata = BuildMetadata(keyMappings, ColumnRole.Key, x => x.CandidateColumn)
                .Union(BuildMetadata(valueMappings, ColumnRole.Value, x => x.CandidateColumn));
            candidate = BuildTable(candidateRows, metadata, dataRowsSamplers["candidate"], "Candidate");
            analysis = BuildMultipleTable(violations, metadata, dataRowsSamplers["analysis"], keyMappings, valueMappings);
        }

        private MarkdownContainer BuildTable(IEnumerable<DataRow> rows, IEnumerable<IColumnDefinition> metadata, ISampler<DataRow> sampler, string title)
        {
            sampler.Build(rows);
            var tableHelper = new StandardTableHelper(sampler.GetResult(), metadata);

            var table = tableHelper.Render();
            return BuildTable(table, rows, title, sampler);
        }

        private MarkdownContainer BuildTable(MarkdownContainer table, IEnumerable<DataRow> rows, string title, ISampler<DataRow> sampler)
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

        private MarkdownContainer BuildMultipleTable(LookupViolationCollection violations, IEnumerable<IColumnDefinition> metadata, ISampler<DataRow> sampler, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings)
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
                container.Append(BuildTable(table, violations.GetRows(state), Textify(state), state == RowViolationState.Mismatch ? null : sampler));
            }
            return container;
        }

        private IEnumerable<IColumnDefinition> BuildMetadata(ColumnMappingCollection mappings, ColumnRole role, Func<ColumnMapping, IColumnIdentifier> identify)
        {
            foreach (var mapping in mappings ?? new ColumnMappingCollection())
                yield return new Column()
                {
                    Identifier = identify.Invoke(mapping),
                    Role = role,
                    Type = mapping.Type,
                };
        }

        protected Paragraph BuildRowCount(int rowCount)
        {
            return ($"Result-set with {rowCount} row{(rowCount > 1 ? "s" : string.Empty)}".ToMarkdownParagraph());
        }

        public virtual string RenderReference() => keysCollectionSamplers["expected"] is NoneSampler<KeyCollection> ? "Display skipped." : reference.ToMarkdown();
        public virtual string RenderCandidate() => keysCollectionSamplers["actual"] is NoneSampler<KeyCollection> ? "Display skipped." : candidate.ToMarkdown();
        public virtual string RenderAnalysis() => keysCollectionSamplers["analysis"] is NoneSampler<KeyCollection> ? "Display skipped." : analysis.ToMarkdown();
        public virtual string RenderPredicate() => "Some references are missing and violate referential integrity";

        public string RenderMessage()
        {
            var sb = new StringBuilder();
            sb.AppendLine(RenderPredicate());
            sb.AppendLine();
            sb.AppendLine(RenderReference());
            sb.AppendLine();
            sb.AppendLine(RenderCandidate());
            sb.AppendLine();
            sb.AppendLine(RenderAnalysis());
            return sb.ToString();
        }

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
