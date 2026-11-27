using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Model.Items;
using NBi.Model.Items.Ranges;
using NBi.Model.Settings;
using NBi.Model.Systems;

namespace NBi.Model.Constraints;

public abstract class AbstractConstraintForCollection : AbstractConstraint
{
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

    public bool IgnoreCase { get; set; } = false;

    public List<string> Items { get; set; } = [];

    public PredefinedItems? PredefinedItems { get; set; }

    public IntegerRange? IntegerRange { get; set; }

    public DateRange? DateRange { get; set; }

    public PatternIntegerRange? PatternIntegerRange { get; set; }

    public ValueRange? Range
    {
        get
        {
            if (PatternIntegerRange != null)
                return PatternIntegerRange;
            if (IntegerRange != null)
                return IntegerRange;
            if (DateRange != null)
                return DateRange;
            return null;
        }
        set
        {
            if (value is PatternIntegerRange patternInteger)
                PatternIntegerRange = patternInteger;
            if (value is IntegerRange integerRange)
                IntegerRange = integerRange;
            if (value is DateRange dateRange)
                DateRange = dateRange;
        }
    }

    public Query? Query { get; set; }

    public Members? Members { get; set; }


    public AbstractConstraintForCollection()
    {
        Items = [];
    }

    public override BaseItem? BaseItem
    {
        get
        {
            if (Query != null)
                return Query;
            if (Members != null && Members.BaseItem != null)
                return Members.BaseItem;
            return null;
        }
    }

}
