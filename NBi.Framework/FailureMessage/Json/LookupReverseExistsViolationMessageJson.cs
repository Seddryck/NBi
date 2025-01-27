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

class LookupReverseExistsViolationMessageJson : LookupExistsViolationMessageJson
{
    public LookupReverseExistsViolationMessageJson(IDictionary<string, ISampler<IResultRow>> samplers)
        : base(samplers) { }

    protected override void RenderAnalysis(LookupViolationCollection violations, IEnumerable<ColumnMetadata> metadata, ISampler<IResultRow> sampler, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings, JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("missing");
        var rows = violations.Values.Where(x => x is LookupExistsViolationInformation)
                            .Cast<LookupExistsViolationInformation>()
                            .SelectMany(x => x.CandidateRows);
        sampler.Build(rows);
        var tableHelper = new StandardTableHelperJson(rows, metadata, sampler);
        tableHelper.Render(writer);
        writer.WriteEndObject();
    }
}
