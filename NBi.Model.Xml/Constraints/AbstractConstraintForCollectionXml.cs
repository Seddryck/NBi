using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Items.Ranges;
using NBi.Model.Xml.Settings;
using NBi.Model.Xml.Systems;

namespace NBi.Model.Xml.Constraints;

public abstract class AbstractConstraintForCollectionXml : AbstractConstraintXml
{
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

    [XmlAttribute("ignore-case")]
    [DefaultValue(false)]
    public bool IgnoreCase { get; set; } = false;

    [XmlElement("item")]
    public List<string> Items { get; set; } = [];

    [XmlElement("predefined")]
    public PredefinedItemsXml? PredefinedItems { get; set; }

    [XmlElement("range-integer")]
    public IntegerRangeXml? IntegerRange { get; set; }

    [XmlElement("range-date")]
    public DateRangeXml? DateRange { get; set; }

    [XmlElement("range-integer-pattern")]
    public PatternIntegerRangeXml? PatternIntegerRange { get; set; }

    [XmlIgnore]
    public RangeXml? Range
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
            if (value is PatternIntegerRangeXml patternInteger)
                PatternIntegerRange = patternInteger;
            if (value is IntegerRangeXml integerRange)
                IntegerRange = integerRange;
            if (value is DateRangeXml dateRange)
                DateRange = dateRange;
        }
    }

    [XmlElement("one-column-query")]
    public QueryXml? Query { get; set; }

    [XmlElement("members")]
    public MembersXml? Members { get; set; }

    public IEnumerable<string> GetItems()
        => Core.Members.MemberSelection.GetItems(Items,
            PredefinedItems == null ? null : new Model.Items.PredefinedItems
            {
                Type = PredefinedItems.Type,
                Language = PredefinedItems.Language,
            },
            Range);

    public AbstractConstraintForCollectionXml()
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
