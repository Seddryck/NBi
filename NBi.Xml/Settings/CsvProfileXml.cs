using NBi.Core;
using NBi.Core.FlatFile;
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

        [XmlAttribute("empty-cell")]
        [DefaultValue("(empty)")]
        public string InternalEmptyCell { get; set; }

        [XmlIgnore]
        public override string EmptyCell
        {
            get
            {
                if (string.IsNullOrEmpty(InternalEmptyCell))
                    return "(empty)";
                return InternalEmptyCell;
            }
            set
            {
                InternalEmptyCell = value;
            }
        }

        [XmlAttribute("missing-cell")]
        [DefaultValue("(null)")]
        public string InternalMissingCell { get; set; }

        [XmlIgnore]
        public override string MissingCell
        {
            get
            {
                if (string.IsNullOrEmpty(InternalMissingCell))
                    return "(null)";
                return InternalMissingCell;
            }
            set
            {
                InternalMissingCell = value;
            }
        }

        public CsvProfileXml(char fieldSeparator, string recordSeparator)
            : base(fieldSeparator, recordSeparator)
        {}

        

    }
}
