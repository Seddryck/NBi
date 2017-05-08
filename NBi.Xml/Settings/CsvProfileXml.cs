using NBi.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Settings
{
    public class CsvProfileXml : CsvProfile
    {
        public CsvProfileXml()
            : base()
        {
            InternalFieldSeparator = ";";
            InternalRecordSeparator = "CrLf";
        }


        [XmlAttribute("field-separator")]
        [DefaultValue(";")]
        public string InternalFieldSeparator { get; set; }

        [XmlIgnore]
        public override char FieldSeparator
        {
            get
            {
                var value = InternalFieldSeparator;
                value = value.Replace("Tab", "\t");
                if (value.Length > 1)
                    throw new ArgumentOutOfRangeException();
                return value[0];
            }
            set
            {
                var stringValue = value.ToString().Replace("\t", "Tab");
                InternalFieldSeparator = stringValue;
            }
        }

        [XmlAttribute("record-separator")]
        [DefaultValue("CrLf")]
        public string InternalRecordSeparator { get; set; }

        [XmlIgnore]
        public override string RecordSeparator 
        {
            get
            {
                var value = InternalRecordSeparator;
                value = value.Replace("Cr", "\r");
                value = value.Replace("Lf", "\n");
                return value;
            }
            set
            {
                value = value.Replace("\r", "Cr");
                value = value.Replace("\n", "Lf");
                InternalRecordSeparator = value;
            }
        }

        [XmlAttribute("first-row-header")]
        [DefaultValue(false)]
        public bool InternalFirstRowHeader { get; set; }

        [XmlIgnore]
        public override bool FirstRowHeader
        {
            get
            {
                return InternalFirstRowHeader;
            }
            set
            {
                InternalFirstRowHeader = value;
            }
        }

        public CsvProfileXml(char fieldSeparator, string recordSeparator)
            : base(fieldSeparator, recordSeparator)
        {}

        

    }
}
