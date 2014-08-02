using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NBi.Core.Report
{
    public class DatabaseParser : IParser
    {
        public string ExtractQuery(IQueryRequest request)
        {
            using (var conn = new SqlConnection())
            {
                //create connection and define sql query
                conn.ConnectionString = request.Source;
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = ReadQueryFromContent();
                
                //create the three parameters for the sql query
                var paramReportPath = new SqlParameter("ReportPath",System.Data.SqlDbType.NVarChar, 425);
                paramReportPath.Value=request.ReportPath;
                cmd.Parameters.Add(paramReportPath);
                var paramReportName = new SqlParameter("ReportName",System.Data.SqlDbType.NVarChar, 425);
                paramReportName.Value=request.ReportName;
                cmd.Parameters.Add(paramReportName);
                //var paramDataSetName = new SqlParameter("DataSetName", System.Data.SqlDbType.NVarChar, 128);
                //paramDataSetName.Value = request.DataSetName;
                //cmd.Parameters.Add(paramDataSetName);
                
                //execute the command
                conn.Open();
                var dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                    throw new ArgumentException(string.Format("No report found on path '{0}' with name '{1}'", request.ReportPath, request.ReportName));

                var dataSetFound = new List<String>();
                while (dr.Read())
                    if (dr.GetString(2) == request.DataSetName)
                        return dr.GetString(5); //CommandText
                    else
                        dataSetFound.Add(dr.GetString(2));
                
                if (dataSetFound.Count()>1)
                    throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The datasets for this report are {3}", request.ReportPath, request.ReportName, request.DataSetName, String.Join(", ", dataSetFound.ToArray())));
                else
                    throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The dataset for this report is {3}", request.ReportPath, request.ReportName, request.DataSetName, dataSetFound[0]));
            }
        }

        private string ReadQueryFromContent()
        {
            var value = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Core.Report.ReportServerDatabaseQuery.sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }
            return value;
        }
    }
}
