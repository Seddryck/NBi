using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Model.Items;
using NBi.Model.Items.ResultSet;
using NBi.Model.Settings;
using NBi.Model.Constraints.Comparer;
using NBi.Model.Items.Calculation;

namespace NBi.Model.Constraints;

public class RowCount : AbstractConstraint
{

    private readonly bool parallelizeQueries;
    public bool ParallelizeQueries
        => parallelizeQueries || (Settings?.ParallelizeQueries ?? true);

    public RowCount()
    {
        this.parallelizeQueries = false;
    }

    internal  RowCount(bool parallelizeQueries)
    {
        this.parallelizeQueries = parallelizeQueries;
    }

    internal RowCount(SuiteSettings settings)
    {
        this.Settings = settings;
    }

    public override Default? Default
    {
        get {return base.Default;}
        set
        {
            base.Default = value;
            //if (Query!=null)
            //    Query.Default=value;
        }
    }

    public LessThan? LessThan { get; set; }
    public Equal? Equal { get; set; }
    public MoreThan? MoreThan { get; set; }

    public Predicate? Comparer
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

    public Filter? Filter { get; set; }
}
