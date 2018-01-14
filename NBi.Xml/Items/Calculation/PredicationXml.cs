using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
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
    public class PredicationXml : IPredicateInfo, IReferencePredicateInfo, ISecondOperandPredicateInfo, ICultureSensitivePredicateInfo, ICaseSensitivePredicateInfo 
    {
        public PredicationXml()
        {
            ColumnIndex = -1;
            ColumnType = ColumnType.Numeric;
        }

        [DefaultValue(-1)]
        [XmlAttribute("column-index")]
        public int ColumnIndex { get; set; }

        [XmlIgnore]
        public bool Not
        {
            get => Predicate.Not; 
            set => Predicate.Not = value;
        }

        [XmlAttribute("operand")]
        public string Operand { get; set; }

        [Obsolete("Deprecated. Use operand in place of name")]
        public string Name { get => Operand; set => Operand=value; }

        [DefaultValue(ColumnType.Numeric)]
        [XmlAttribute("type")]
        public ColumnType ColumnType { get; set; }

        [XmlElement(Type = typeof(LessThanXml), ElementName ="less-than")]
        [XmlElement(Type = typeof(MoreThanXml), ElementName = "more-than")]
        [XmlElement(Type = typeof(EqualXml), ElementName = "equal")]
        [XmlElement(Type = typeof(NullXml), ElementName = "null")]
        [XmlElement(Type = typeof(EmptyXml), ElementName = "empty")]
        [XmlElement(Type = typeof(LowerCaseXml), ElementName = "lower-case")]
        [XmlElement(Type = typeof(UpperCaseXml), ElementName = "upper-case")]
        [XmlElement(Type = typeof(StartsWithXml), ElementName = "starts-with")]
        [XmlElement(Type = typeof(EndsWithXml), ElementName = "ends-with")]
        [XmlElement(Type = typeof(ContainsXml), ElementName = "contains")]
        [XmlElement(Type = typeof(MatchesRegexXml), ElementName = "matches-regex")]
        [XmlElement(Type = typeof(MatchesNumericXml), ElementName = "matches-numeric")]
        [XmlElement(Type = typeof(MatchesDateXml), ElementName = "matches-date")]
        [XmlElement(Type = typeof(MatchesTimeXml), ElementName = "matches-time")]
        [XmlElement(Type = typeof(WithinRangeXml), ElementName = "within-range")]
        [XmlElement(Type = typeof(WithinListXml), ElementName = "within-list")]
        [XmlElement(Type = typeof(IntegerXml), ElementName = "integer")]
        [XmlElement(Type = typeof(ModuloXml), ElementName = "modulo")]
        [XmlElement(Type = typeof(OnTheDayXml), ElementName = "on-the-day")]
        [XmlElement(Type = typeof(OnTheHourXml), ElementName = "on-the-hour")]
        [XmlElement(Type = typeof(OnTheMinuteXml), ElementName = "on-the-minute")]
        [XmlElement(Type = typeof(TrueXml), ElementName = "true")]
        [XmlElement(Type = typeof(FalseXml), ElementName = "false")]
        public PredicateXml Predicate { get; set; }
        
        [XmlIgnore]
        public object Reference
        {
            get { return Predicate.Value ?? Predicate.Values as object; }
            set { Predicate.Value = value.ToString(); }
        }

        [XmlIgnore]
        public object SecondOperand
        {
            get { return (Predicate as ITwoOperandsXml)?.SecondOperand; }
        }

        [XmlIgnore]
        public StringComparison StringComparison
        {
            get
            {
                if (Predicate is CaseSensitiveTextPredicateXml)
                    return ((CaseSensitiveTextPredicateXml)Predicate).IgnoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
                else
                    throw new InvalidOperationException();
            }
        }

        [XmlIgnore]
        public string Culture
        {
            get
            {
                if (Predicate is CultureSensitiveTextPredicateXml)
                    return ((CultureSensitiveTextPredicateXml)Predicate).Culture;
                else
                    throw new InvalidOperationException();
            }
        }

        [XmlIgnore]
        public ComparerType ComparerType { get => Predicate.ComparerType; }
    }
}
