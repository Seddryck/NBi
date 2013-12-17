﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Items.Ranges;
using NBi.Xml.Settings;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints
{
    public abstract class AbstractConstraintForCollectionXml : AbstractConstraintXml
    {
        public override DefaultXml Default
        {
            get { return base.Default; }
            set
            {
                base.Default = value;
                if (Query != null)
                    Query.Default = value;
            }
        }

        [XmlAttribute("ignore-case")]
        [DefaultValue(false)]
        public bool IgnoreCase { get; set; }

        [XmlElement("item")]
        public List<string> Items { get; set; }

        [XmlElement("predefined")]
        public PredefinedItemsXml PredefinedItems { get; set; }

        [XmlElement("range-integer")]
        public IntegerRangeXml IntegerRange { get; set; }

        [XmlElement("range-date")]
        public DateRangeXml DateRange { get; set; }

        [XmlElement("range-integer-pattern")]
        public PatternIntegerRangeXml PatternIntegerRange { get; set; }

        [XmlIgnore]
        public RangeXml Range
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
                if (value is PatternIntegerRangeXml)
                    PatternIntegerRange = (PatternIntegerRangeXml)value;
                if (value is IntegerRangeXml)
                    IntegerRange = (IntegerRangeXml)value;
                if (value is DateRangeXml)
                    DateRange = (DateRangeXml)value;
            }
        }

        [XmlElement("one-column-query")]
        public QueryXml Query { get; set; }

        [XmlElement("members")]
        public MembersXml Members { get; set; }

        public IEnumerable<string> GetItems()
        {
            var list = new List<string>();
            if (Items != null)
                list.AddRange(Items);
            if (PredefinedItems != null)
                list.AddRange(PredefinedItems.GetItems());
            if (Range != null)
                list.AddRange(Range.GetItems());

            return list;
        }

        public AbstractConstraintForCollectionXml()
        {
            Items = new List<string>();
        }
    }
}
