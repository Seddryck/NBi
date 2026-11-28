using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Model.Items;
using NBi.Model.Items.ResultSet;
using NBi.Model.Settings;
using NBi.Model.Items.Hierarchical.Xml;
using NBi.Model.Systems;

namespace NBi.Model.Constraints;

public class EqualTo : AbstractConstraint
{
    public enum ComparisonBehavior
    {
        MultipleRows = 0,
        SingleRow = 1
    }

    public EqualTo()
    {
        parallelizeQueries = false;
        ValuesDefaultType = ColumnType.Numeric;
    }

    internal EqualTo(bool parallelizeQueries)
        => this.parallelizeQueries = parallelizeQueries;

    internal EqualTo(SuiteSettings settings)
        => Settings = settings;

    public override Default? Default
    {
        get => base.Default;
        set
        {
            base.Default = value;
            if (Query != null)
                Query.Default = value;
        }
    }

    public virtual ResultSet? ResultSetOld { get; set; }

    public virtual ResultSetSystem? ResultSet { get; set; }

    public Query? Query { get; set; }

    public XmlSource? XmlSource { get; set; }

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

    public virtual ComparisonBehavior Behavior { get; set; } = ComparisonBehavior.MultipleRows;

    public KeySelection KeysDef { get; set; } = KeySelection.First;

    public ValueSelection ValuesDef { get; set; } = ValueSelection.AllExceptFirst;

    public string? KeyName { get; set; }

    public string? ValueName { get; set; }

    public ColumnType ValuesDefaultType { get; set; } = ColumnType.Numeric;

    protected bool isToleranceSpecified;
    public bool IsToleranceSpecified
    {
        get { return isToleranceSpecified; }
        protected set { isToleranceSpecified = value; }
    }

    protected string? tolerance;
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

    public List<ColumnDefinition> columnsDef = [];

    public IReadOnlyList<IColumnDefinition> ColumnsDef
        => columnsDef.Cast<IColumnDefinition>().ToList();

    private readonly bool parallelizeQueries;
    public bool ParallelizeQueries => parallelizeQueries || (Settings?.ParallelizeQueries ?? true);
}
