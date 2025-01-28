using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Sampling;

public class SamplersFactory<T>
{
    public IDictionary<string, ISampler<T>> Instantiate(IFailureReportProfile profile)
    {
        return new Dictionary<string, ISampler<T>>()
        {
            {"expected", SelectSampler(profile.ExpectedSet, profile.ThresholdSampleItem, profile.MaxSampleItem) },
            {"actual", SelectSampler(profile.ActualSet, profile.ThresholdSampleItem, profile.MaxSampleItem) },
            {"analysis", SelectSampler(profile.AnalysisSet, profile.ThresholdSampleItem, profile.MaxSampleItem) },
        };
    }

    public IDictionary<string, ISampler<T>> InstantiateLookup(IFailureReportProfile profile)
    {
        return new Dictionary<string, ISampler<T>>()
        {
            {"reference", SelectSampler(profile.ExpectedSet, profile.ThresholdSampleItem, profile.MaxSampleItem) },
            {"candidate", SelectSampler(profile.ActualSet, profile.ThresholdSampleItem, profile.MaxSampleItem) },
            {"analysis", SelectSampler(profile.AnalysisSet, profile.ThresholdSampleItem, profile.MaxSampleItem) },
        };
    }

    protected virtual ISampler<T> SelectSampler(FailureReportSetType type, int thresholdSampleItem, int maxSampleItem)
    {
        return type switch
        {
            FailureReportSetType.None => new NoneSampler<T>(),
            FailureReportSetType.Sample => new BasicSampler<T>(thresholdSampleItem, maxSampleItem),
            FailureReportSetType.Full => new FullSampler<T>(),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
