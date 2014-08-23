﻿using System;
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
            
            //Load the xml
            var docXml = new XmlDocument();
            docXml.Load(fullPath);

            //Check that the data set exist
            var xpath = string.Format("//rd:Report/rd:DataSets/rd:DataSet[@Name=\"{0}\"]", request.DataSetName);

            var nsmgr = new XmlNamespaceManager(docXml.NameTable);
            nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");

            var node = docXml.SelectSingleNode(xpath, nsmgr);
            if (node == null)
                throw BuildDataSetNotFoundException(request, docXml, "//rd:Report/rd:DataSets/rd:DataSet", nsmgr);

            //Search in the xml the DataSet and especially the CommandText within this dataset
            xpath = string.Format("//rd:Report/rd:DataSets/rd:DataSet[@Name=\"{0}\"]/rd:Query/rd:CommandText", request.DataSetName);

            node = docXml.SelectSingleNode(xpath, nsmgr);
            if (node != null)
                return node.InnerText; // Weve fond the query
            
            //If not found then we'll check if it's not a shared dataset
            xpath = string.Format("//rd:Report/rd:DataSets/rd:DataSet[@Name=\"{0}\"]/rd:SharedDataSet/rd:SharedDataSetReference", request.DataSetName);
            node = docXml.SelectSingleNode(xpath, nsmgr);
            if (node == null)
                throw new ArgumentException(string.Format("The data set named '{0}' has been found but no command text or shared dataset reference has been found", request.DataSetName));

            //If it's a shared dataset then we need to file the correspoding file
            var sharedDataSetName = node.InnerText + ".rds";
            fullPath = string.Format("{0}{1}", request.ReportPath, sharedDataSetName);
            if (!File.Exists(fullPath))
                throw new ArgumentException(string.Format("Cannot find the file for the shared dataSet named '{0}'", sharedDataSetName));

            //If the file is found then we need to select the query inside the file
            docXml.Load(fullPath);
            xpath = string.Format("//rd:SharedDataSet/rd:DataSet[@Name=\"\"]/rd:Query/rd:CommandText");
            node = docXml.SelectSingleNode(xpath, nsmgr);
            if (node != null)
                return node.InnerText;

            throw new ArgumentException(string.Format("Cannot find the command text in the shared dataSet named '{0}'", sharedDataSetName));
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
