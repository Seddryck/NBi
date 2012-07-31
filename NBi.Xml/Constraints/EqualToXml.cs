using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Xml.Constraints.EqualTo;

namespace NBi.Xml.Constraints
{
    public class EqualToXml : AbstractConstraintXml
    {
        [XmlAttribute("resultSet-file")]
        public string ResultSetFile { get; set; }

        [XmlAttribute("query-file")]
        public string QueryFile { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("connectionString-ref")]
        public string ConnectionStringReference { get; set; }

        [XmlText]
        public string InlineQuery { get; set; }

        public string Query
        {
            get
            {
                //if both are empty return null
                if (string.IsNullOrEmpty(InlineQuery) && string.IsNullOrEmpty(QueryFile))
                    return null;

                //if Sql is specified then return it
                if (!string.IsNullOrEmpty(InlineQuery))
                    return InlineQuery;

                //Else read the file's content and 
                var query = File.ReadAllText(QueryFile);
                return query;
            }
        }

        [XmlElement("resultSet")]
        public ResultSetXml ResultSet { get; set; }

        [XmlElement("key")]
        public List<KeyXml> Keys { get; set; }

        [XmlElement("value")]
        public List<ValueXml> Values { get; set; }



        public IDbCommand Command
        {
            get
            {
                var query = Query;
                if (query == null)
                    return null;

                var conn = ConnectionFactory.Get(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = query;

                return cmd;
            }
        }
    }
}
