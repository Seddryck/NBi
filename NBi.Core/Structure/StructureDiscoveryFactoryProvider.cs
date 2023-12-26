﻿using Microsoft.AnalysisServices;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Client;
using NBi.Core.Structure.Olap;
using NBi.Core.Structure.Relational;
using NBi.Core.Structure.Tabular;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NBi.Core.Structure
{
    public class StructureDiscoveryFactoryProvider
    {
        public const string Olap = "olap";
        public const string Relational = "relational";
        public const string Tabular = "tabular";

        protected IDictionary<string, Type> dico;
        public StructureDiscoveryFactoryProvider()
	    {
            dico = new Dictionary<string, Type>();
            Initialize();
	    }
        
        protected virtual void Initialize()
        {
            dico.Add(Olap, typeof(OlapStructureDiscoveryFactory));
            dico.Add(Relational, typeof(RelationalStructureDiscoveryFactory));
            dico.Add(Tabular, typeof(TabularStructureDiscoveryFactory));
        }

        public IStructureDiscoveryFactory Instantiate(string connectionString)
        {
            var sessionFactory = new ClientProvider();
            var connection = sessionFactory.Instantiate(connectionString).CreateNew() as IDbConnection;
            var dbType = MapConnectionTypeToDatabaseType(connection);

            if (!dico.Keys.Contains(dbType))
                throw new ArgumentException();

            var factoryType = dico[dbType];
            var ctor = factoryType.GetConstructor(new Type[]{typeof(IDbConnection)});
            var factory = (IStructureDiscoveryFactory)ctor.Invoke(new object[] { connection });

            return factory;
        }

        protected virtual string MapConnectionTypeToDatabaseType(IDbConnection connection)
        {
            if (connection is SqlConnection)
                return Relational;
            if (connection is OleDbConnection)
                return Relational;
            if (connection is OdbcConnection)
                return Relational;
            if (connection is AdomdConnection)
                return InquireFurtherAnalysisService(connection.ConnectionString);
            throw new ArgumentOutOfRangeException();
        }

        protected virtual string InquireFurtherAnalysisService(string connectionString)
        {
            try
            {
                var parsedMode = string.Empty;
                using (var conn = new AdomdConnection(connectionString))
                {
                    conn.Open();
                    var restrictions = new AdomdRestrictionCollection
                    {
                        new AdomdRestriction("ObjectExpansion", "ReferenceOnly")
                    };
                    var ds = conn.GetSchemaDataSet("DISCOVER_XML_METADATA", restrictions);
                    var xml = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);
                    parsedMode = ParseXmlaResponse(doc);
                }


                switch (parsedMode)
                {
                    case "Default": return Olap;
                    case "Multidimensional": return Olap;
                    case "SharePoint": return Tabular;
                    case "Tabular": return Tabular;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceWarning,"Can't detect server mode for SSAS, using Olap. Initial message:" + ex.Message);
                return Olap;
            }
            return Olap;

        }

        protected string ParseXmlaResponse(XmlDocument doc)
        {
            var root = doc.DocumentElement;

            var nm = new XmlNamespaceManager(doc.NameTable);
            nm.AddNamespace("ddl300", "http://schemas.microsoft.com/analysisservices/2011/engine/300");
            nm.AddNamespace("default", "http://schemas.microsoft.com/analysisservices/2003/engine");
            var serverModeNode = root.SelectSingleNode("//ddl300:ServerMode", nm);
            if (serverModeNode == null)
            {
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, "Trying to detect the server mode for SSAS but the server doesn't return this information. Trying to get it from version.");
                var versionNode = root.SelectSingleNode("//default:Version", nm);
                if (versionNode != null)
                {
                    var splitVersion = versionNode.InnerText.Split('.');
                    short releaseVersion = 0;
                    if (splitVersion.Count() >= 1)
                        if (short.TryParse(splitVersion[0], out releaseVersion))
                            if (releaseVersion < 11)
                                return "Multidimensional";
                    throw new ArgumentException(string.Format("Unable to locate the node for 'ServerMode' and can't guess based on node 'Version'. Value returned for version is '{0}'. Use AdomdClient 12.0 or higher.", versionNode.InnerText));        
                }
                throw new ArgumentException("Unable to locate the node for 'ServerMode' or the node for 'Version'. Use AdomdClient 12.0 or higher.");
            }
                
            return serverModeNode.InnerText;
        }
    }
}
