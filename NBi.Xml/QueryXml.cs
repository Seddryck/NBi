using System.Data;
using System.IO;
using System.Xml.Serialization;
using NBi.Core;

namespace NBi.Xml
{
    public class QueryXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("query-File")]
        public string Filename { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("connectionString-Ref")]
        public string ConnectionStringReference { get; set; }

        [XmlText]
        public string InlineQuery { get; set; }

        public string Query
        {
            get
            {
                //if Sql is specified then return it
                if (!string.IsNullOrEmpty(InlineQuery))
                    return InlineQuery;

                //Else read the file's content and 
                var query = File.ReadAllText(Filename);
                return query;
            }
        }

        public IDbCommand Instantiate()
        {
            var conn = ConnectionFactory.Get(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = Query;

            return cmd;
        }


    }
}
