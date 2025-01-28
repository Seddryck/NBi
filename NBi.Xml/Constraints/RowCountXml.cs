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
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation;

namespace NBi.Xml.Constraints;

public class RowCountXml : AbstractConstraintXml
{

    private readonly bool parallelizeQueries;
    public bool ParallelizeQueries
        => parallelizeQueries || (Settings?.ParallelizeQueries ?? true);

    public RowCountXml()
    {
        this.parallelizeQueries = false;
    }

    internal  RowCountXml(bool parallelizeQueries)
    {
        this.parallelizeQueries = parallelizeQueries;
    }

    internal RowCountXml(SettingsXml settings)
    {
        this.Settings = settings;
    }

    [XmlIgnore()]
    public override DefaultXml? Default
    {
        get {return base.Default;} 
        set
        {
            base.Default = value;
            //if (Query!=null)
            //    Query.Default=value;
        }
    }

    [XmlElement("less-than", Order = 1)]
    public LessThanXml? LessThan { get; set; }
    [XmlElement("equal", Order = 2)]
    public EqualXml? Equal { get; set; }
    [XmlElement("more-than", Order = 3)]
    public MoreThanXml? MoreThan { get; set; }

    [XmlIgnore]
    public PredicateXml? Comparer
    {
        get
        {
            if (Equal != null)
                return Equal;
            if (MoreThan != null)
                return MoreThan;
            if (LessThan != null)
                return LessThan;
            return null;
        }
    }

    [XmlElement("filter", Order = 0)]
    public FilterXml? Filter { get; set; }
}
