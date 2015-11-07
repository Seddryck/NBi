using NBi.Core.ResultSet;
using NBi.Xml.Constraints.Comparer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Validate
{
    public class ComparisonXml
    {
        public ComparisonXml()
        {
            ColumnIndex = -1;
            ColumnType = ColumnType.Numeric;
        }

        [DefaultValue(-1)]
        [XmlAttribute("column-index")]
        public int ColumnIndex { get; set; }

        [XmlAttribute("formula")]
        public string Formula { get; set; }

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
                return null;
            }
        }
    }
}
