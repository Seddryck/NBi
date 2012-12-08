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

        protected bool isToleranceSpecified;
        [XmlIgnore()]
        public bool IsToleranceSpecified
        {
            get { return isToleranceSpecified; }
            protected set { isToleranceSpecified = value; }
        }

        protected decimal tolerance;
        [XmlAttribute("tolerance")]
        public decimal Tolerance
        {
            get
            { return tolerance; }

            set
            {
                tolerance = value;
                isToleranceSpecified = true;
            }
        }

        [XmlElement("column")]
        public List<ColumnXml> columnsDef;

        public IList<IColumn> ColumnsDef
        {
            get
            {
                if (columnsDef == null)
                    columnsDef = new List<ColumnXml>();
                return columnsDef.Cast<IColumn>().ToList();
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
