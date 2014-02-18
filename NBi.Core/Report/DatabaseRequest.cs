using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Report
{
    public class DatabaseRequest : IQueryRequest
    {
        private readonly string connectionString;
        private readonly string reportPath;
        private readonly string reportName;
        private readonly string dataSetName;

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        public string ReportPath
        {
            get
            {
                return reportPath;
            }
        }

        public string ReportName
        {
            get
            {
                return reportName;
            }
        }

        public string DataSetName
        {
            get
            {
                return dataSetName;
            }
        }

        public DatabaseRequest(string connectionString, string reportPath, string reportName, string dataSetName)
        {
            this.connectionString = connectionString;
            this.reportPath = reportPath;
            this.reportName = reportName;
            this.dataSetName = dataSetName;
        }
    }
}
