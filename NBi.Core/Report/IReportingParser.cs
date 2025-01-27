using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.Core.Report;

public interface IReportingParser
{
    ReportingCommand ExtractCommand(ReportDataSetRequest request);
    ReportingCommand ExtractCommand(SharedDatasetRequest request);
}
