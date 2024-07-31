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

namespace NBi.Framework.FailureMessage.Markdown
{
    class LookupExistsViolationMessageMarkdown : LookupViolationMessageMarkdown
    {

        public LookupExistsViolationMessageMarkdown(IDictionary<string, ISampler<IResultRow>> samplers)
            : base(samplers) { }

        protected override void RenderAnalysis(LookupViolationCollection violations, IEnumerable<ColumnMetadata> metadata, ISampler<IResultRow> sampler, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings, MarkdownContainer container)
        {
            if (violations.Values.Count == 0)
            {
                container.Append("Analysis".ToMarkdownHeader());
                var state = violations.Values.Select(x => x.State).First();
                container.Append(GetExplanationText(violations, state).ToMarkdownParagraph());

                var rows = violations.Values.Where(x => x is LookupExistsViolationInformation)
                            .Cast<LookupExistsViolationInformation>()
                            .SelectMany(x => x.CandidateRows);
                sampler.Build(rows);

                var tableHelper = new StandardTableHelperMarkdown(rows, metadata, sampler);
                tableHelper.Render(container);
            }
        }
    }
}
