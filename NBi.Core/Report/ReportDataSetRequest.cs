using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Report;

public class ReportDataSetRequest
{
    public string Source { get; set; }
    public string Path { get; set; }
    public string ReportName { get; set; }
    public string DataSetName { get; set; }

    public ReportDataSetRequest(string source, string path, string reportName, string dataSetName)
    {
        Source = source;
        Path = path;
        ReportName = reportName;
        DataSetName = dataSetName;
    }
}
