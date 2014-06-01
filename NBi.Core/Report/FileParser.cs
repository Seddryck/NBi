using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace NBi.Core.Report
{
    class FileParser : IParser
    {
        public string ExtractQuery(IQueryRequest request)
        {
            var fullPath = string.Format("{0}{1}", request.ReportPath, request.ReportName);
            if (!File.Exists(fullPath))
                throw new ArgumentException(string.Format("No report found on path '{0}' with name '{1}'", request.ReportPath, request.ReportName));
            
            var docXml = new XmlDocument();
            docXml.Load(fullPath);
            var xpath = string.Format("//rd:Report/rd:DataSets/rd:DataSet[@Name=\"{0}\"]/rd:Query/rd:CommandText", request.DataSetName);

            var nsmgr = new XmlNamespaceManager(docXml.NameTable);
            nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");

            var node = docXml.SelectSingleNode(xpath, nsmgr);
            if (node != null)
                return node.InnerText;
            else
                throw BuildDataSetNotFoundException(request, docXml, "//rd:Report/rd:DataSets/rd:DataSet", nsmgr);
        }

        private Exception BuildDataSetNotFoundException(IQueryRequest request, XmlDocument docXml, string xpath, XmlNamespaceManager nsmgr)
        {
            var nodes = docXml.SelectNodes(xpath, nsmgr);
            var dataSetFound = new List<String>();
            foreach (XmlNode node in nodes)
                dataSetFound.Add(node.Attributes["Name"].Value);

            if (dataSetFound.Count() > 1)
                throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The datasets for this report are {3}", request.ReportPath, request.ReportName, request.DataSetName, String.Join(", ", dataSetFound.ToArray())));
            else
                throw new ArgumentException(string.Format("The requested dataset ('{2}') wasn't found for the report on path '{0}' with name '{1}'. The dataset for this report is named '{3}'", request.ReportPath, request.ReportName, request.DataSetName, dataSetFound[0]));
        }
    }
}
