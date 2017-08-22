using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NBi.Xml.Items.Xml;

namespace NBi.Xml.Constraints
{
    public class EqualToXml : AbstractConstraintXml
    {
        public enum ComparisonBehavior
        {
            [XmlEnum("multiple-rows")]
            MultipleRows = 0,
            [XmlEnum("single-row")]
            SingleRow = 1
        }


        public EqualToXml()
        {
            parallelizeQueries = false;
            ValuesDefaultType = ColumnType.Numeric;
        }

        internal  EqualToXml(bool parallelizeQueries)
            : this()
        {
            this.parallelizeQueries = parallelizeQueries;
        }

        internal EqualToXml(SettingsXml settings)
            : this()
        {
            this.Settings = settings;
        }

        [XmlIgnore()]
        public override DefaultXml Default
        {
            get {return base.Default;} 
            set
            {
                base.Default = value;
                if (Query!=null)
                    Query.Default=value;
            }
        }

        [XmlElement("resultSet")]
        public ResultSetXml ResultSet { get; set; }

        [XmlElement(Type = typeof(QueryXml), ElementName = "query"),
        ]
        public QueryXml Query { get; set; }

        [XmlElement("xml-source")]
        public XmlSourceXml XmlSource { get; set; }

        public override BaseItem BaseItem
        {
            get
            {
                if (Query != null)
                    return Query;
                if (ResultSet != null)
                    return ResultSet;
                if (XmlSource != null)
                    return XmlSource;

                return null;
            }
        }

        [XmlAttribute("behavior")]
        [DefaultValue(ComparisonBehavior.MultipleRows)]
        public ComparisonBehavior Behavior { get; set; }

        [XmlAttribute("keys")]
        [DefaultValue(SettingsResultSetComparisonByIndex.KeysChoice.First)]
        public SettingsResultSetComparisonByIndex.KeysChoice KeysDef { get; set; }

        [XmlAttribute("values")]
        [DefaultValue(SettingsResultSetComparisonByIndex.ValuesChoice.AllExpectFirst)]
        public SettingsResultSetComparisonByIndex.ValuesChoice ValuesDef { get; set; }

        [XmlAttribute("keys-names")]
        public string KeyName { get; set; }

        [XmlAttribute("values-names")]
        public string ValueName { get; set; }

        [XmlAttribute("values-default-type")]
        [DefaultValue(ColumnType.Numeric)]
        public ColumnType ValuesDefaultType { get; set; }

        protected bool isToleranceSpecified;
        [XmlIgnore()]
        public bool IsToleranceSpecified
        {
            get { return isToleranceSpecified; }
            protected set { isToleranceSpecified = value; }
        }

        protected string tolerance;
        [XmlAttribute("tolerance")]
        [DefaultValue("")]
        public string Tolerance
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
        public List<NBi.Xml.Items.ResultSet.ColumnDefinitionXml> columnsDef;

        public IReadOnlyList<IColumnDefinition> ColumnsDef
        {
            get
            {
                if (columnsDef == null)
                    columnsDef = new List<NBi.Xml.Items.ResultSet.ColumnDefinitionXml>();
                return columnsDef.Cast<IColumnDefinition>().ToList();
            }
        }

        [XmlAttribute("persistance")]
        [DefaultValue(PersistanceChoice.Never)]
        public PersistanceChoice Persistance;

        public ISettingsResultSetComparison GetSettings()
        {
            var builder = new ResultSetComparisonBuilder();
            builder.Setup(
                    Behavior == ComparisonBehavior.MultipleRows
                    , KeysDef
                    , KeyName
                    , ValuesDef
                    , ValueName
                    , ValuesDefaultType
                    , new NumericToleranceFactory().Instantiate(Tolerance)
                    , ColumnsDef);
            builder.Build();
            return builder.GetSettings();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public virtual IDbCommand GetCommand()
        {
            if (Query==null)
                return null;

            var conn = new ConnectionFactory().Get(Query.GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = Query.GetQuery();
            

            return cmd;
        }

        private readonly bool parallelizeQueries;
        public bool ParallelizeQueries
        {
            get
            {
                return parallelizeQueries || Settings.ParallelizeQueries;
            }
        }
              
    }
}
