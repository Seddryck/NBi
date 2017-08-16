using NBi.Core.Calculation;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints.Comparer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation
{
    public class PredicateXml : IPredicateInfo
    {
        public PredicateXml()
        {
            ColumnIndex = -1;
            ColumnType = ColumnType.Numeric;
        }

        [DefaultValue(-1)]
        [XmlAttribute("column-index")]
        public int ColumnIndex { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [DefaultValue(false)]
        [XmlAttribute("not")]
        public bool Not { get; set; }

        [DefaultValue(ColumnType.Numeric)]
        [XmlAttribute("type")]
        public ColumnType ColumnType { get; set; }

        [XmlElement("less-than")]
        public LessThanXml LessThan { get; set; }
        [XmlElement("equal")]
        public EqualXml Equal { get; set; }
        [XmlElement("more-than")]
        public MoreThanXml MoreThan { get; set; }
        [XmlElement("null")]
        public NullXml Null { get; set; }
        [XmlElement("empty")]
        public EmptyXml Empty { get; set; }
        [XmlElement("starts-with")]
        public StartsWithXml StartsWith { get; set; }
        [XmlElement("ends-with")]
        public EndsWithXml EndsWith { get; set; }
        [XmlElement("contains")]
        public ContainsXml Contains { get; set; }

        [XmlIgnore]
        public AbstractComparerXml Comparer
        {
            get
            {
                if (Equal != null)
                    return Equal;
                if (MoreThan != null)
                    return MoreThan;
                if (LessThan != null)
                    return LessThan;
                if (Null != null)
                    return Null;
                if (Empty != null)
                    return Empty;
                if (StartsWith != null)
                    return StartsWith;
                if (EndsWith != null)
                    return EndsWith;
                if (Contains != null)
                    return Contains;
                return null;
            }
        }

        [XmlIgnore]
        public ComparerType ComparerType
        {
            get
            {
                if (Equal != null)
                    return ComparerType.Equal;
                if (MoreThan != null)
                    if (MoreThan.OrEqual)
                        return ComparerType.MoreThanOrEqual;
                    else
                        return ComparerType.MoreThan;
                if (LessThan != null)
                    if (LessThan.OrEqual)
                        return ComparerType.LessThanOrEqual;
                    else
                        return ComparerType.LessThan;
                if (Null != null)
                    return ComparerType.Null;
                if (Empty != null)
                    if (Empty.OrNull)
                        return ComparerType.NullOrEmpty;
                    else
                        return ComparerType.Empty;
                if (StartsWith != null)
                    return ComparerType.StartsWith;
                if (EndsWith != null)
                    return ComparerType.EndsWith;
                if (Contains != null)
                    return ComparerType.Contains;
                return ComparerType.Equal;
            }
        }

        [XmlIgnore]
        public object Reference
        {
            get { return Comparer.Value; }
        }
    }
}
