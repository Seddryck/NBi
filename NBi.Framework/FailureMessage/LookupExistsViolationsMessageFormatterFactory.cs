using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Framework.FailureMessage.Json;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework.Sampling;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using NBi.Extensibility;

namespace NBi.Framework.FailureMessage;

public class LookupExistsViolationsMessageFormatterFactory
{
    public virtual ILookupViolationMessageFormatter Instantiate(IFailureReportProfile profile)
    {
        var dataRowsFactory = new SamplersFactory<IResultRow>();
        var dataRowsSamplers = dataRowsFactory.InstantiateLookup(profile);

        return profile.Format switch
        {
            FailureReportFormat.Markdown => new LookupExistsViolationMessageMarkdown(dataRowsSamplers),
            FailureReportFormat.Json => new LookupExistsViolationMessageJson(dataRowsSamplers),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
