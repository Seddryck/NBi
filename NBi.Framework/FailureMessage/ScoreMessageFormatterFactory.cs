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

namespace NBi.Framework.FailureMessage
{
    public class ScoreMessageFormatterFactory
    {
        public IScoreMessageFormatter Instantiate()
        {
            switch (profile.Format)
            {
                case FailureReportFormat.Markdown:
                    return new ScoreMessageMarkdown();
                case FailureReportFormat.Json:
                    return new ScoreMessageJson();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}
