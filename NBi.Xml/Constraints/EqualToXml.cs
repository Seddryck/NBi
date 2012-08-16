using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints.EqualTo;
using NBi.Xml.Systems;

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

        private string _query;

        public string GetQuery()
        {
            //if Sql is specified then return it
            if (!string.IsNullOrEmpty(InlineQuery))
                return InlineQuery;

            //Else read the file's content (if local varaible not populated)
            if (!string.IsNullOrEmpty(QueryFile))
            {
                if (string.IsNullOrEmpty(_query))
                    _query = File.ReadAllText(QueryFile);
                return _query;
            }

            //Else use the QueryXml object
            if (Query != null)
            {
                return Query.GetQuery();
            }

            return null;
        }

        public string GetConnectionString()
        {
            //if ConnectionString is specified then return it
            if (!string.IsNullOrEmpty(ConnectionString))
                return ConnectionString;

            //Else use the QueryXml object
            if (Query != null)
            {
                return Query.ConnectionString;
            }

            //Else use the default value
            if (!string.IsNullOrEmpty(Default.ConnectionString))
                return Default.ConnectionString;

            return null;
        }

        [XmlElement("resultSet")]
        public ResultSetXml ResultSet { get; set; }

        [XmlElement("query")]
        public QueryXml Query { get; set; }

        [XmlAttribute("keys")]
        public ResultSetComparaisonSettings.KeysChoice KeysDef { get; set; }

        [XmlAttribute("values")]
        public ResultSetComparaisonSettings.ValuesChoice ValuesDef { get; set; }

        [XmlAttribute("tolerance")]
        public decimal Tolerance { get; set; }

        [XmlElement("column")]
        public List<ColumnXml> _columnsDef { get; set; }

        public IList<IColumn> ColumnsDef
        {
            get
            {
                if (_columnsDef == null)
                    _columnsDef = new List<ColumnXml>();
                return _columnsDef.Cast<IColumn>().ToList();
            }
        }

        public ResultSetComparaisonSettings GetSettings()
        {
            return new ResultSetComparaisonSettings(KeysDef, ValuesDef, Tolerance, ColumnsDef);
        }

        public IDbCommand GetCommand()
        {
            if (string.IsNullOrEmpty(GetQuery()))
                return null;

            var conn = ConnectionFactory.Get(GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = GetQuery();

            return cmd;
        }

        
    }
}
