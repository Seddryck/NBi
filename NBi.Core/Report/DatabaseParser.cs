using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NBi.Core.Report
{
    public class DatabaseParser : IParser
    {
        public ReportCommand ExtractQuery(IQueryRequest request)
        {
            var otherDataSets = new List<string>();
            var query = SearchDataSet(
                request.Source
                , request.ReportPath
                , request.ReportName
                , request.DataSetName
                , ref otherDataSets);
            if (query == null)
            {
                var reference = SearchSharedDataSet(
                    request.Source
                    , request.ReportPath
                    , request.ReportName
                    , request.DataSetName
                    , ref otherDataSets);
                if (!string.IsNullOrEmpty(reference))
                    query = ReadQueryFromSharedDataSet(request.Source, reference);
            }

            if (query == null)
                return query;

            if (otherDataSets.Count() == 0)
                throw new ArgumentException(string.Format("No report found on path '{0}' with name '{1}'", request.ReportPath, request.ReportName));
            else if (otherDataSets.Count() == 1)
                throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The dataset for this report is {3}", request.ReportPath, request.ReportName, request.DataSetName, otherDataSets[0]));
            else
                throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The datasets for this report are {3}", request.ReportPath, request.ReportName, request.DataSetName, String.Join(", ", otherDataSets.ToArray())));
        }

        private ReportCommand SearchDataSet(string source, string reportPath, string reportName, string dataSetName, ref List<string> otherDataSets)
        {
            using (var conn = new SqlConnection())
            {
                //create connection and define sql query
                conn.ConnectionString = source;
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = ReadQueryFromContent("ListDataSet");

                //create the three parameters for the sql query
                var paramReportPath = new SqlParameter("ReportPath", System.Data.SqlDbType.NVarChar, 425);
                paramReportPath.Value = reportPath;
                cmd.Parameters.Add(paramReportPath);
                var paramReportName = new SqlParameter("ReportName", System.Data.SqlDbType.NVarChar, 425);
                paramReportName.Value = reportName;
                cmd.Parameters.Add(paramReportName);

                //execute the command
                conn.Open();
                var dr = cmd.ExecuteReader();
                
                while (dr.Read())
                    if (dr.GetString(2) == dataSetName)
                    {
                        var command = new ReportCommand();
                        command.CommandType = (CommandType)Enum.Parse(typeof(CommandType), dr.GetString(4)); //CommandType
                        command.Text = dr.GetString(5); //CommandText
                        return command;
                    }
                    else
                        otherDataSets.Add(dr.GetString(2));
            }
            return null;
        }

        private string SearchSharedDataSet(string source, string reportPath, string reportName, string dataSetName, ref List<string> otherDataSets)
        {
            using (var conn = new SqlConnection())
            {
                //create connection and define sql query
                conn.ConnectionString = source;
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = ReadQueryFromContent("ListSharedDataSet");

                //create the three parameters for the sql query
                var paramReportPath = new SqlParameter("ReportPath", System.Data.SqlDbType.NVarChar, 425);
                paramReportPath.Value = reportPath;
                cmd.Parameters.Add(paramReportPath);
                var paramReportName = new SqlParameter("ReportName", System.Data.SqlDbType.NVarChar, 425);
                paramReportName.Value = reportName;
                cmd.Parameters.Add(paramReportName);

                //execute the command
                conn.Open();
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                    if (dr.GetString(2) == dataSetName)
                        return dr.GetString(3);
                    else
                        otherDataSets.Add(dr.GetString(2));
            }
            return null;
        }

        private ReportCommand ReadQueryFromSharedDataSet(string source, string reference)
        {
            using (var conn = new SqlConnection())
            {
                //create connection and define sql query
                conn.ConnectionString = source;
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = ReadQueryFromContent("QueryFromSharedDataSet");

                //create the three parameters for the sql query
                var paramReference = new SqlParameter("SharedDataSetName", System.Data.SqlDbType.NVarChar, 425);
                paramReference.Value = reference;
                cmd.Parameters.Add(paramReference);

                //execute the command
                conn.Open();
                var dr = cmd.ExecuteReader();

                
                if (dr.Read())
                {
                    var command = new ReportCommand();
                    command.CommandType = (CommandType)Enum.Parse(typeof(CommandType), dr.GetString(2)) ; //CommandType
                    command.Text = dr.GetString(3); //CommandText
                    return command;
                }
            }
            return null;
        }


        private string ReadQueryFromContent(string name)
        {
            var value = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Core.Report.ReportServer" + name + ".sql"))
            using (StreamReader reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }
            return value;
        }
    }
}
