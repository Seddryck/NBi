using System.Text;
using System.Xml.Serialization;
using NBi.Core;

namespace NBi.Xml.Systems
{
    public class QueryXml : AbstractSystemUnderTestXml
    {
        public virtual bool IsQuery()
        {
            return true;
        }
        
        //TODO should be removed in 1.1 and inheriting from BaseQueryXml ... (issue with desrialization of interface)
        //public DefaultXml Default { get; set; }

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

            //Else read the file's content and 
            var query = System.IO.File.ReadAllText(File, Encoding.UTF8);
            return query;
        }

        public virtual string GetConnectionString()
        {
            //if Sql is specified then return it
            if (!string.IsNullOrEmpty(ConnectionString))
                return ConnectionString;

            //Else get the reference ConnectionString 
            if (!string.IsNullOrEmpty(ConnectionStringReference))
                return Settings.GetReference(ConnectionStringReference).ConnectionString;

            //Else get the default ConnectionString 
            if (Default!=null && !string.IsNullOrEmpty(Default.ConnectionString))
                return Default.ConnectionString;
            return null;
        }

    }
}
