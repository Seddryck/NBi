using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
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

public class ScoreMessageFormatterFactory
{
    public virtual IScoreMessageFormatter Instantiate(IFailureReportProfile profile)
    {
        return profile.Format switch
        {
            FailureReportFormat.Markdown => new ScoreMessageMarkdown(),
            FailureReportFormat.Json => new ScoreMessageJson(),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
