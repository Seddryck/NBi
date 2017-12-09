using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Sampling
{
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

        private ISampler<T> SelectSampler(FailureReportSetType type, int thresholdSampleItem, int maxSampleItem)
        {
            switch (type)
            {
                case FailureReportSetType.None:
                    return new NoneSampler<T>();
                case FailureReportSetType.Sample:
                    return new BasicSampler<T>(thresholdSampleItem, maxSampleItem);
                case FailureReportSetType.Full:
                    return new FullSampler<T>();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
