using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Framework.FailureMessage.Common;
using NBi.Framework.FailureMessage.Json.Helper;
using NBi.Framework.Sampling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Json
{
    class LookupViolationMessageJson : LookupViolationMessage<string>
    {
        public LookupViolationMessageJson(IDictionary<string, ISampler<DataRow>> samplers)
            : base(samplers) { }


        protected override string RenderStandardTable(IEnumerable<DataRow> rows, IEnumerable<ColumnMetadata> metadata, ISampler<DataRow> sampler, string title)
        {
            sampler.Build(rows);

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (var writer = new JsonTextWriter(sw))
                new StandardTableHelperJson(rows, metadata, sampler).Render(writer);
            return sb.ToString();
        }

        protected override string RenderAnalysis(LookupViolationCollection violations, IEnumerable<ColumnMetadata> metadata, ISampler<DataRow> sampler, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (var writer = new JsonTextWriter(sw))
            {
                foreach (var state in violations.Values.Select(x => x.State).Distinct())
                {
                    writer.WriteStartObject();
                    if (state == RowViolationState.Mismatch)
                    {
                        var fullSampler = new FullSampler<LookupMatchesViolationComposite>();
                        var rows = violations.Values.Where(x => x is LookupMatchesViolationInformation)
                                .Cast<LookupMatchesViolationInformation>()
                                .SelectMany(x => x.CandidateRows);
                        fullSampler.Build(rows);
                        new LookupTableHelperJson(rows, metadata, fullSampler).Render(writer);
                    }
                    else
                    {
                        var rows = violations.Values.Where(x => x is LookupExistsViolationInformation)
                                .Cast<LookupExistsViolationInformation>()
                                .SelectMany(x => x.CandidateRows);
                        sampler.Build(rows);
                        new StandardTableHelperJson(rows, metadata, sampler).Render(writer);
                    }
                    writer.WriteEndObject();
                }
            }
            return sb.ToString();
        }

        public override string RenderMessage()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("timestamp");
                writer.WriteValue(DateTime.Now);
                if (!string.IsNullOrEmpty(reference))
                {
                    writer.WritePropertyName("reference");
                    writer.WriteRawValue(reference);
                }
                if (!string.IsNullOrEmpty(candidate))
                {
                    writer.WritePropertyName("candidate");
                    writer.WriteRawValue(candidate);
                }
                if (!string.IsNullOrEmpty(analysis))
                {
                    writer.WritePropertyName("analysis");
                    writer.WriteRawValue(analysis);
                }
                writer.WriteEndObject();
                return sb.ToString();
            }
        }

        public override string RenderReference() => reference;
        public override string RenderCandidate() => candidate;
        public override string RenderAnalysis() => analysis;
    }
}
