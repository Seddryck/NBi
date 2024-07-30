using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;

namespace NBi.Core.Report
{
    class FileReportingParser : IReportingParser
    {
        public ReportingCommand ExtractCommand(ReportDataSetRequest request)
        {
            var reportName = request.ReportName.EndsWith(".rdl") ? request.ReportName : request.ReportName + ".rdl";

            var fullPath = string.Format("{0}{1}{2}", request.Source, request.Path, reportName);
            if (!File.Exists(fullPath))
                throw new ArgumentException(string.Format("No report found on path '{0}{1}' with name '{2}'", request.Source, request.Path, request.ReportName));

            //Load the xml
            var docXml = new XmlDocument();
            docXml.Load(fullPath);
            var root = docXml.FirstChild ?? throw new NullReferenceException();
            if (root.NodeType == XmlNodeType.XmlDeclaration)
                root = root.NextSibling;

            //Check that the data set exist
            var xpath = string.Format("//rd:Report/rd:DataSets/rd:DataSet[@Name=\"{0}\"]", request.DataSetName);
            //var xpath = "//Report";

            var nsmgr = new XmlNamespaceManager(docXml.NameTable);
            nsmgr.AddNamespace("rd", root?.GetNamespaceOfPrefix(string.Empty) ?? string.Empty);

            var node = docXml.SelectSingleNode(xpath, nsmgr);
            if (node == null)
                throw BuildDataSetNotFoundException(request, docXml, "//rd:Report/rd:DataSets/rd:DataSet", nsmgr);

            //Search in the xml the DataSet and especially the CommandText within this dataset
            xpath = string.Format("//rd:Report/rd:DataSets/rd:DataSet[@Name=\"{0}\"]/rd:Query/rd:CommandText", request.DataSetName);

            node = docXml.SelectSingleNode(xpath, nsmgr);
            if (node != null)
            {
                var text = node.InnerText; // Weve fond the query
                var reportCommand = new ReportingCommand() { Text = text };

                xpath = string.Format("//rd:Report/rd:DataSets/rd:DataSet[@Name=\"{0}\"]/rd:Query/rd:CommandType", request.DataSetName);
                node = docXml.SelectSingleNode(xpath, nsmgr);
                if (node == null)
                    reportCommand.CommandType = CommandType.Text;
                else
                    reportCommand.CommandType = (CommandType)Enum.Parse(typeof(CommandType), node.InnerText);
                return reportCommand;
            }

            //If not found then we'll check if it's not a shared dataset
            xpath = string.Format("//rd:Report/rd:DataSets/rd:DataSet[@Name=\"{0}\"]/rd:SharedDataSet/rd:SharedDataSetReference", request.DataSetName);
            node = docXml.SelectSingleNode(xpath, nsmgr);
            if (node == null)
                throw new ArgumentException(string.Format("The data set named '{0}' has been found but no command text or shared dataset reference has been found", request.DataSetName));

            var sharedDataSetName = node.InnerText + ".rsd";
            var subRequest = new SharedDatasetRequest
            (
                request.Source,
                request.Path,
                sharedDataSetName
            );
            return ExtractCommand(subRequest);
        }

        public ReportingCommand ExtractCommand(SharedDatasetRequest request)
        {
            var reportName = request.SharedDatasetName.EndsWith(".rsd") ? request.SharedDatasetName : request.SharedDatasetName + ".rsd";

            var fullPath = string.Format("{0}{1}{2}", request.Source, request.Path, reportName);
            if (!File.Exists(fullPath))
                throw new ArgumentException(string.Format("No shared dataset found on path '{0}{1}' with name '{2}'", request.Source, request.Path, reportName));

            //If the file is found then we need to select the query inside the file
            var docXml = new XmlDocument();
            docXml.Load(fullPath);

            var root = docXml.FirstChild ?? throw new NullReferenceException();
            if (root.NodeType == XmlNodeType.XmlDeclaration)
                root = root.NextSibling;

            var xpath = string.Format("//rd:SharedDataSet/rd:DataSet[@Name=\"\"]/rd:Query/rd:CommandText");
            var nsmgr = new XmlNamespaceManager(docXml.NameTable);
            nsmgr.AddNamespace("rd", root?.GetNamespaceOfPrefix(string.Empty) ?? string.Empty);
            var node = docXml.SelectSingleNode(xpath, nsmgr);
            if (node != null)
            {
                var text = node.InnerText; // We've found the query
                var reportCommand = new ReportingCommand() { Text = text };

                xpath = string.Format("//rd:SharedDataSet/rd:DataSet[@Name=\"\"]/rd:Query/rd:CommandType");
                node = docXml.SelectSingleNode(xpath, nsmgr);
                if (node == null)
                    reportCommand.CommandType = CommandType.Text;
                else
                    reportCommand.CommandType = (CommandType)Enum.Parse(typeof(CommandType), node.InnerText);
                return reportCommand;
            }

            throw new ArgumentException(string.Format("Cannot find the command text in the shared dataSet at '{0}'", fullPath));
        }

        private Exception BuildDataSetNotFoundException(ReportDataSetRequest request, XmlDocument docXml, string xpath, XmlNamespaceManager nsmgr)
        {
            var nodes = docXml.SelectNodes(xpath, nsmgr);
            var dataSetFound = new List<string>();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                    dataSetFound.Add(node.Attributes?["Name"]?.Value ?? throw new NullReferenceException());
            }
            if (dataSetFound.Count > 1)
                throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The datasets for this report are {3}", request.Path, request.ReportName, request.DataSetName, String.Join(", ", [.. dataSetFound])));
            else
                throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The dataset for this report is named '{3}'", request.Path, request.ReportName, request.DataSetName, dataSetFound[0]));
        }
    }
}
