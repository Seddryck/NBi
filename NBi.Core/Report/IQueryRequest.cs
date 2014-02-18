using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Report
{
    public interface IQueryRequest
    {
        string ConnectionString { get; }
        string ReportPath { get; }
        string ReportName { get; }
        string DataSetName { get; }
    }
}
