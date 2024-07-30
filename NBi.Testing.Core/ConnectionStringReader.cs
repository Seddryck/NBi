using System;
using System.Xml;

namespace NBi.Testing
{
    class ConnectionStringReader
    {
        public static string Get(string name)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(GetFilename());
            XmlNodeList nodes = xmldoc.GetElementsByTagName("add");
            foreach (XmlNode node in nodes)
                if (node.Attributes?["name"]?.Value == name)
                    return node.Attributes?["connectionString"]?.Value ?? string.Empty;
            throw new Exception();
        }


        public static string GetOleDbCube()
        {
            return Get("OleDbCube");
        }

        public static string GetOleDbSql()
        {
            return Get("OleDbSql");
        }

        public static string GetOdbcSql()
        {
            return Get("OdbcSql");
        }

        public static string GetLocalOleDbSql()
        {
            return Get("LocalOleDbSql");
        }

        public static string GetLocalOdbcSql()
        {
            return Get("LocalOdbcSql");
        }

        public static string GetAdomd()
        {
            return Get("Adomd");
        }

        public static string GetSqlClient()
        {
            return Get("SqlClient");
        }

        private static string GetFilename()
        {
            //If available use the user file
            if (System.IO.File.Exists($@"{FileOnDisk.GetDirectoryPath()}\ConnectionString.user.config"))
                return $@"{FileOnDisk.GetDirectoryPath()}\ConnectionString.user.config";
            else if (System.IO.File.Exists($@"{FileOnDisk.GetDirectoryPath()}\ConnectionString.config"))
                return $@"{FileOnDisk.GetDirectoryPath()}\ConnectionString.config";
            return "";
        }

        internal static string GetAdomdTabular()
        {
            return Get("AdomdTabular");
        }


        internal static string GetLocalSqlClient()
        {
            return Get("LocalSqlClient");
        }
        internal static string GetReportServerDatabase()
        {
            return Get("ReportServerDatabase");
        }

        internal static string GetIntegrationServer()
        {
            return Get("IntegrationServer");
        }

        internal static string GetIntegrationServerTargetDatabase()
        {
            return Get("IntegrationServerTargetDatabase");
        }

    }
}
