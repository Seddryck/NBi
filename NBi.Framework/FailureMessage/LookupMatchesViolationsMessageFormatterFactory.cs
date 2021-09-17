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

namespace NBi.Framework.FailureMessage
{
    public class LookupMatchesViolationsMessageFormatterFactory
    {
        public ILookupViolationMessageFormatter Instantiate(IFailureReportProfile profile)
        {
            var dataRowsFactory = new SamplersFactory<IResultRow>();
            var dataRowsSamplers = dataRowsFactory.InstantiateLookup(profile);

            var keysCollectionFactory = new SamplersFactory<KeyCollection>();
            var keysCollectionSamplers = keysCollectionFactory.Instantiate(profile);

            switch (profile.Format)
            {
                case FailureReportFormat.Markdown:
                    return new LookupMatchesViolationMessageMarkdown(dataRowsSamplers);
                case FailureReportFormat.Json:
                    return new LookupMatchesViolationMessageJson(dataRowsSamplers);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
