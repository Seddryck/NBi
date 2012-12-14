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

        [XmlAttribute("persistance")]
        public PersistanceChoice Persistance;

        public ResultSetComparisonSettings GetSettings()
        {
            return new ResultSetComparisonSettings(KeysDef, ValuesDef, Tolerance, ColumnsDef);
        }

        public virtual IDbCommand GetCommand()
        {
            if (Query==null)
                return null;

            var conn = ConnectionFactory.Get(Query.GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = Query.GetQuery();

            return cmd;
        }

        
    }
}
