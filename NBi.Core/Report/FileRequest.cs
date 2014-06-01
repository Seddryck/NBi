using System;
using System.IO;
using System.Linq;

namespace NBi.Core.Report
{
    public class FileRequest : IQueryRequest
    {
        private readonly string reportPath;
        private readonly string reportName;
        private readonly string dataSetName;

        public string Source
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public string ReportPath
        {
            get
            {
                if (reportPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    return reportPath;
                else
                    return reportPath + Path.DirectorySeparatorChar;
            }
        }

        public string ReportName
        {
            get
            {
                if (reportName.EndsWith(".rdl"))
                    return reportName;
                else
                    return reportName + ".rdl";
            }
        }

        public string DataSetName
        {
            get
            {
                return dataSetName;
            }
        }

        public FileRequest(string reportPath, string reportName, string dataSetName)
        {
            this.reportPath = reportPath;
            this.reportName = reportName;
            this.dataSetName = dataSetName;
        }
    }
}
