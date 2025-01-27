using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Report;

public class ReportingParserFactory
{
    public virtual IReportingParser Instantiate(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return new FileReportingParser();
        else
            return new DatabaseReportingParser();
    }
}
