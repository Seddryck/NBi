using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.Framework.FailureMessage.Common;
using NBi.Framework.FailureMessage.Common.Helper;
using NBi.Framework.FailureMessage.Markdown.Helper;
using NBi.Framework.Sampling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown;

class LookupMatchesViolationMessageMarkdown : LookupViolationMessageMarkdown
{

    public LookupMatchesViolationMessageMarkdown(IDictionary<string, ISampler<IResultRow>> samplers)
        : base(samplers) { }

    protected override void RenderAnalysis(LookupViolationCollection violations, IEnumerable<ColumnMetadata> metadata, ISampler<IResultRow> sampler, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings, MarkdownContainer container)
    {
        container.Append("Analysis".ToMarkdownHeader());
        var state = violations.Values.Select(x => x.State).First();
        container.Append(GetExplanationText(violations, state).ToMarkdownParagraph());

        var fullSampler = new FullSampler<LookupMatchesViolationComposite>();
        var rows = violations.Values.Where(x => x is LookupMatchesViolationInformation)
                .Cast<LookupMatchesViolationInformation>()
                .SelectMany(x => x.CandidateRows);
        fullSampler.Build(rows);
        var tableHelper = new LookupTableHelperMarkdown(rows, metadata, fullSampler);
        tableHelper.Render(container);
    }
}
