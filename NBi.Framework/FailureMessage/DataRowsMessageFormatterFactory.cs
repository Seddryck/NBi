using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Extensibility;
using NBi.Framework.FailureMessage.Json;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework.Sampling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage;

public class DataRowsMessageFormatterFactory
{
    public virtual IDataRowsMessageFormatter Instantiate(IFailureReportProfile profile, Core.ResultSet.EngineStyle style)
    {
        var factory = new SamplersFactory<IResultRow>();
        var samplers = factory.Instantiate(profile);

        return profile.Format switch
        {
            FailureReportFormat.Markdown => new DataRowsMessageMarkdown(style, samplers),
            FailureReportFormat.Json => new DataRowsMessageJson(style, samplers),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
