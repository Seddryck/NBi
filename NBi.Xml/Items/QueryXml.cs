using System;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Xml.Settings;

namespace NBi.Xml.Items
{
    public class QueryXml
    {
        public DefaultXml Default { get; set; }
        
        [XmlAttribute("file")]
        public string File { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("connectionString-ref")]
        public string ConnectionStringReference { get; set; }

        [XmlText]
        public string InlineQuery { get; set; }

        public virtual string GetQuery()
        {
            //if Sql is specified then return it
            if (!string.IsNullOrEmpty(InlineQuery))
                return InlineQuery;

            //Else check that file exists and read the file's content
            if (!System.IO.File.Exists(File))
                throw new ExternalDependencyNotFoundException(File);
            var query = System.IO.File.ReadAllText(File, Encoding.UTF8);
            return query;
        }

        public virtual string GetConnectionString()
        {
            //if ConnectionString is specified then return it
            if (!string.IsNullOrEmpty(ConnectionString))
                return ConnectionString;

            //Else get the reference ConnectionString 
            //if (!string.IsNullOrEmpty(ConnectionStringReference))
                //return Settings.GetReference(ConnectionStringReference).ConnectionString;

            //Else get the default ConnectionString 
            if (Default != null && !string.IsNullOrEmpty(Default.ConnectionString))
                return Default.ConnectionString;
            return null;
        }
    }
}
