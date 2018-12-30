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

namespace NBi.Framework.FailureMessage
{
    public class LookupViolationsMessageFormatterFactory
    {
        public ILookupViolationsMessageFormatter Instantiate(IFailureReportProfile profile)
        {
            var dataRowsFactory = new SamplersFactory<DataRow>();
            var dataRowsSamplers = dataRowsFactory.Instantiate(profile);

            var keysCollectionFactory = new SamplersFactory<KeyCollection>();
            var keysCollectionSamplers = keysCollectionFactory.Instantiate(profile);

            switch (profile.Format)
            {
                case FailureReportFormat.Markdown:
                    return new LookupViolationsMessageMarkdown(keysCollectionSamplers, dataRowsSamplers);
                case FailureReportFormat.Json:
                    return new ReferenceViolationsMessageJson(dataRowsSamplers);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
