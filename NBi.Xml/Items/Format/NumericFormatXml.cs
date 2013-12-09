using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Format;

namespace NBi.Xml.Items.Format
{
    public class NumericFormatXml : INumericFormat
    {
        [XmlAttribute("decimal-digits")]
        public int DecimalDigits { get; set; }

        [XmlAttribute("decimal-separator")]
        [DefaultValue(".")]
        public string DecimalSeparator { get; set; }

        [XmlAttribute("group-separator")]
        [DefaultValue(",")]
        public string GroupSeparator { get; set; }

        public NumericFormatXml() : base()
        {
            DecimalSeparator = ".";
            GroupSeparator = ",";
        }
    }
}
