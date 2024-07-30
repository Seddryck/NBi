using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NBi.Core.Report
{
    public class DatabaseReportingParser : IReportingParser
    {
        public ReportingCommand ExtractCommand(ReportDataSetRequest request)
        {
            var otherDataSets = new List<string>();
            var query = SearchDataSet(
                request.Source
                , request.Path
                , request.ReportName
                , request.DataSetName
                , ref otherDataSets);
            if (query == null)
            {
                var reference = SearchSharedDataSet(
                    request.Source
                    , request.Path
                    , request.ReportName
                    , request.DataSetName
                    , ref otherDataSets);
                if (!string.IsNullOrEmpty(reference))
                    query = ReadQueryFromSharedDataSet(request.Source, request.Path, reference);
            }

            if (query != null)
                return query;

            if (otherDataSets.Count == 0)
                throw new ArgumentException(string.Format("No report found on path '{0}' with name '{1}'", request.Path, request.ReportName));
            else if (otherDataSets.Count == 1)
                throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The dataset for this report is {3}", request.Path, request.ReportName, request.DataSetName, otherDataSets[0]));
            else
                throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The datasets for this report are {3}", request.Path, request.ReportName, request.DataSetName, String.Join(", ", [.. otherDataSets])));
        }

        public ReportingCommand ExtractCommand(SharedDatasetRequest request)
        {
            var query = ReadQueryFromSharedDataSet(request.Source, request.Path, request.SharedDatasetName);
            if (query != null)
                return query;

            throw new ArgumentException(string.Format("The requested shared dataset ('{1}') wasn't found on path '{0}'.", request.Path, request.SharedDatasetName));
        }

        private ReportingCommand? SearchDataSet(string source, string reportPath, string reportName, string dataSetName, ref List<string> otherDataSets)
        {
            using var conn = new SqlConnection();
            //create connection and define sql query
            conn.ConnectionString = source;
            var cmd = new SqlCommand
            {
                Connection = conn,
                CommandText = ReadQueryFromContent("ListDataSet")
            };

            //create the three parameters for the sql query
            var paramReportPath = new SqlParameter("ReportPath", SqlDbType.NVarChar, 425)
            {
                Value = reportPath
            };
            cmd.Parameters.Add(paramReportPath);
            var paramReportName = new SqlParameter("ReportName", SqlDbType.NVarChar, 425)
            {
                Value = reportName
            };
            cmd.Parameters.Add(paramReportName);

            //execute the command
            conn.Open();
            var dr = cmd.ExecuteReader();

            while (dr.Read())
                if (dr.GetString(2) == dataSetName)
                {
                    var command = new ReportingCommand
                    {
                        CommandType = (CommandType)Enum.Parse(typeof(CommandType), dr.GetString(4)), //CommandType
                        Text = dr.GetString(5) //CommandText
                    };
                    return command;
                }
                else
                    otherDataSets.Add(dr.GetString(2));
            return null;
        }

        private string? SearchSharedDataSet(string source, string reportPath, string reportName, string dataSetName, ref List<string> otherDataSets)
        {
            using var conn = new SqlConnection();
            //create connection and define sql query
            conn.ConnectionString = source;
            var cmd = new SqlCommand
            {
                Connection = conn,
                CommandText = ReadQueryFromContent("ListSharedDataSet")
            };

            //create the three parameters for the sql query
            var paramReportPath = new SqlParameter("ReportPath", System.Data.SqlDbType.NVarChar, 425)
            {
                Value = reportPath
            };
            cmd.Parameters.Add(paramReportPath);
            var paramReportName = new SqlParameter("ReportName", System.Data.SqlDbType.NVarChar, 425)
            {
                Value = reportName
            };
            cmd.Parameters.Add(paramReportName);

            //execute the command
            conn.Open();
            var dr = cmd.ExecuteReader();

            while (dr.Read())
                if (dr.GetString(2) == dataSetName)
                    return dr.GetString(3);
                else
                    otherDataSets.Add(dr.GetString(2));
            return null;
        }

        private ReportingCommand? ReadQueryFromSharedDataSet(string source, string path, string reference)
        {
            using var conn = new SqlConnection();
            //create connection and define sql query
            conn.ConnectionString = source;
            var cmd = new SqlCommand
            {
                Connection = conn,
                CommandText = ReadQueryFromContent("QueryFromSharedDataSet")
            };

            //create the two parameters for the sql query
            var paramPath = new SqlParameter("SharedDataSetPath", SqlDbType.NVarChar, 425)
            {
                Value = path
            };
            cmd.Parameters.Add(paramPath);


            var paramReference = new SqlParameter("SharedDataSetName", SqlDbType.NVarChar, 425)
            {
                Value = reference
            };
            cmd.Parameters.Add(paramReference);

            //execute the command
            conn.Open();
            var dr = cmd.ExecuteReader();


            if (dr.Read())
            {
                var command = new ReportingCommand
                {
                    CommandType = (CommandType)Enum.Parse(typeof(CommandType), dr.GetString(2)), //CommandType
                    Text = dr.GetString(3) //CommandText
                };
                return command;
            }
            return null;
        }


        private string ReadQueryFromContent(string name)
        {
            var value = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Core.Report.ReportServer" + name + ".sql") ?? throw new NullReferenceException())
            using (StreamReader reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }
            return value;
        }
    }
}
