using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.Framework.FailureMessage.Common;
using NBi.Framework.FailureMessage.Common.Helper;
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

namespace NBi.Framework.FailureMessage.Json;

class LookupMatchesViolationMessageJson : LookupViolationMessageJson
{
    public LookupMatchesViolationMessageJson(IDictionary<string, ISampler<IResultRow>> samplers)
        : base(samplers) { }

    protected override void RenderAnalysis(LookupViolationCollection violations, IEnumerable<ColumnMetadata> metadata, ISampler<IResultRow> sampler, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings, JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("non-matching");
        var localSampler = new FullSampler<LookupMatchesViolationComposite>();
        var rows = violations.Values.Where(x => x is LookupMatchesViolationInformation)
                            .Cast<LookupMatchesViolationInformation>()
                            .SelectMany(x => x.CandidateRows);
        localSampler.Build(rows);
        var tableHelper = new LookupTableHelperJson(rows, metadata, localSampler);
        tableHelper.Render(writer);
        writer.WriteEndObject();
    }
}
