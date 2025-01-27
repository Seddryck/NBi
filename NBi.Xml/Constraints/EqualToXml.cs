using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Xml.Items;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NBi.Xml.Items.Hierarchical.Xml;
using NBi.Core.ResultSet.Equivalence;
using NBi.Core.Query.Client;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints;

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

    internal EqualToXml(bool parallelizeQueries)
        => this.parallelizeQueries = parallelizeQueries;

    internal EqualToXml(SettingsXml settings)
        => Settings = settings;

    [XmlIgnore()]
    public override DefaultXml? Default
    {
        get => base.Default;
        set
        {
            base.Default = value;
            if (Query != null)
                Query.Default = value;
        }
    }

    [XmlElement("resultSet")]
    public virtual ResultSetXml? ResultSetOld { get; set; }

    [XmlElement("result-set")]
    public virtual ResultSetSystemXml? ResultSet { get; set; }

    [XmlElement(Type = typeof(QueryXml), ElementName = "query"),
    ]
    public QueryXml? Query { get; set; }

    [XmlElement("xml-source")]
    public XmlSourceXml? XmlSource { get; set; }

    public override BaseItem? BaseItem
    {
        get
        {
            if (Query != null)
                return Query;
            if (ResultSetOld != null)
                return ResultSetOld;
            if (XmlSource != null)
                return XmlSource;

            return null;
        }
    }

    [XmlAttribute("behavior")]
    [DefaultValue(ComparisonBehavior.MultipleRows)]
    public virtual ComparisonBehavior Behavior { get; set; } = ComparisonBehavior.MultipleRows;

    [XmlAttribute("keys")]
    [DefaultValue(SettingsOrdinalResultSet.KeysChoice.First)]
    public SettingsOrdinalResultSet.KeysChoice KeysDef { get; set; } = SettingsOrdinalResultSet.KeysChoice.First;

    [XmlAttribute("values")]
    [DefaultValue(SettingsOrdinalResultSet.ValuesChoice.AllExpectFirst)]
    public SettingsOrdinalResultSet.ValuesChoice ValuesDef { get; set; } = SettingsOrdinalResultSet.ValuesChoice.AllExpectFirst;

    [XmlAttribute("keys-names")]
    public string? KeyName { get; set; }

    [XmlAttribute("values-names")]
    public string? ValueName { get; set; }

    [XmlAttribute("values-default-type")]
    [DefaultValue(ColumnType.Numeric)]
    public ColumnType ValuesDefaultType { get; set; } = ColumnType.Numeric;

    protected bool isToleranceSpecified;
    [XmlIgnore()]
    public bool IsToleranceSpecified
    {
        get { return isToleranceSpecified; }
        protected set { isToleranceSpecified = value; }
    }

    protected string? tolerance;
    [XmlAttribute("tolerance")]
    [DefaultValue("")]
    public virtual string? Tolerance
    {
        get
        { return tolerance; }

        set
        {
            tolerance = string.IsNullOrEmpty(value) ? string.Empty : value;
            isToleranceSpecified = !string.IsNullOrEmpty(value);
        }
    }

    [XmlElement("column")]
    public List<ColumnDefinitionXml> columnsDef = [];

    public IReadOnlyList<IColumnDefinition> ColumnsDef
        => columnsDef.Cast<IColumnDefinition>().ToList();

    private readonly bool parallelizeQueries;
    public bool ParallelizeQueries => parallelizeQueries || (Settings?.ParallelizeQueries ?? true);
}

public class EqualToOldXml : EqualToXml { }
