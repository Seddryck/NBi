using System.Data;
using System.IO;
using System.Xml.Serialization;
using NBi.Core;

namespace NBi.Xml.Constraints
{
    public class EqualToXml : AbstractConstraintXml
    {
        [XmlAttribute("resultSet-File")]
        public string ResultSetFile { get; set; }

        [XmlAttribute("query-File")]
        public string QueryFile { get; set; }

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
                var query = File.ReadAllText(QueryFile);
                return query;
            }
        }

        public IDbCommand Command
        {
            get
            {
                var conn = ConnectionFactory.Get(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = Query;

                return cmd;
            }
        }
    }
}
