using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Framework.FailureMessage.Json;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework.Sampling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public class ItemsMessageFormatterFactory
    {
        public virtual IItemsMessageFormatter Instantiate(IFailureReportProfile profile)
        {
            var factory = new SamplersFactory<string>();
            var samplers = factory.Instantiate(profile);

            return profile.Format switch
            {
                FailureReportFormat.Markdown => new ItemsMessageMarkdown(samplers),
                FailureReportFormat.Json => new ItemsMessageJson(samplers),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
