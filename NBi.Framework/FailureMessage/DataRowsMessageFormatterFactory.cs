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

namespace NBi.Framework.FailureMessage
{
    public class DataRowsMessageFormatterFactory
    {
        public IDataRowsMessageFormatter Instantiate(IFailureReportProfile profile, Core.ResultSet.EngineStyle style)
        {
            var factory = new SamplersFactory<IResultRow>();
            var samplers = factory.Instantiate(profile);

            switch (profile.Format)
            {
                case FailureReportFormat.Markdown:
                    return new DataRowsMessageMarkdown(style, samplers);
                case FailureReportFormat.Json:
                    return new DataRowsMessageJson(style, samplers);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}
