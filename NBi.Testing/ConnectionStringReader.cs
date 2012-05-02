using System.IO;
using System.Xml;

namespace NBi.Testing
{
    class ConnectionStringReader
    {
        public static string Get()
        {            
            var xmldoc = new XmlDocument();
            xmldoc.Load(GetFilename());
            XmlNode node = xmldoc.GetElementsByTagName("add").Item(0);
            return node.Attributes["connectionString"].Value;
        }

        private static string GetFilename()
        {
            //If available use the user file
            if (System.IO.File.Exists("ConnectionString.user.config"))
            {
                return "ConnectionString.user.config";
            }
            else if (System.IO.File.Exists("ConnectionString.config"))
            {
                return "ConnectionString.config";
            }
            return "";
        }
    }
}
