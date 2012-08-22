using System.Collections.Generic;
using System.Data;
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
        public ResultSetComparisonSettings.KeysChoice KeysDef { get; set; }

        [XmlAttribute("values")]
        public ResultSetComparisonSettings.ValuesChoice ValuesDef { get; set; }

        protected bool _isToleranceSpecified;
        [XmlIgnore()]
        public bool IsToleranceSpecified
        {
            get { return _isToleranceSpecified; }
            protected set { _isToleranceSpecified = value; }
        }

        protected decimal _tolerance;
        [XmlAttribute("tolerance")]
        public decimal Tolerance
        {
            get
            { return _tolerance; }

            set
            {
                _tolerance = value;
                _isToleranceSpecified = true;
            }
        }

        [XmlElement("column")]
        public List<ColumnXml> _columnsDef;

        public IList<IColumn> ColumnsDef
        {
            get
            {
                if (_columnsDef == null)
                    _columnsDef = new List<ColumnXml>();
                return _columnsDef.Cast<IColumn>().ToList();
            }
        }

        public ResultSetComparisonSettings GetSettings()
        {
            return new ResultSetComparisonSettings(KeysDef, ValuesDef, Tolerance, ColumnsDef);
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
