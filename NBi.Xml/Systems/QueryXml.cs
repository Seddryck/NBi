using System.IO;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Xml.Settings;

namespace NBi.Xml.Systems
{
    public class QueryXml : AbstractSystemUnderTestXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        public override object Instantiate()
        {
            var conn = ConnectionFactory.Get(GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = GetQuery();

            return cmd;
        }


        //TODO should be removed in 1.1 and inheriting from BaseQueryXml ... (issue with desrialization of interface)
        //public DefaultXml Default { get; set; }

        [XmlAttribute("query-file")]
        public string Filename { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("connectionString-ref")]
        public string ConnectionStringReference { get; set; }

        [XmlText]
        public string InlineQuery { get; set; }

        public string GetQuery()
        {
            //if Sql is specified then return it
            if (!string.IsNullOrEmpty(InlineQuery))
                return InlineQuery;

            //Else read the file's content and 
            var query = File.ReadAllText(Filename);
            return query;
        }

        public string GetConnectionString()
        {
            //if Sql is specified then return it
            if (!string.IsNullOrEmpty(ConnectionString))
                return ConnectionString;

            //Else get the reference ConnectionString 
            if (!string.IsNullOrEmpty(Default.ConnectionString))
                return Default.ConnectionString;
            return null;
        }

    }
}
