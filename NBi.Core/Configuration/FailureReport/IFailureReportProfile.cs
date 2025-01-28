using NBi.Core.Configuration.FailureReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration;

public interface IFailureReportProfile
{
    int MaxSampleItem { get; set; }
    int ThresholdSampleItem { get; set; }

    FailureReportSetType ExpectedSet { get; set; }
    FailureReportSetType ActualSet { get; set; }
    FailureReportSetType AnalysisSet { get; set; }

    FailureReportFormat Format { get; set; }
    FailureReportMode Mode { get; set; }

}
