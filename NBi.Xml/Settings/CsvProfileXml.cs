using NBi.Core;
using NBi.Core.FlatFile;
using NBi.Extensibility.FlatFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Settings
{
    public class CsvProfileXml : IFlatFileProfile
    {
        public CsvProfileXml()
            : this(';', "CrLf")
        { }

        public CsvProfileXml(char fieldSeparator, string recordSeparator)
        {
            FieldSeparator = fieldSeparator;
            RecordSeparator = recordSeparator;
        }

        [XmlAttribute("field-separator")]
        [DefaultValue(";")]
        public string InternalFieldSeparator { get; set; }

        [XmlIgnore]
        public char FieldSeparator
        {
            get => (InternalFieldSeparator.Replace("Tab", "\t").Length <= 1 ? InternalFieldSeparator.Replace("Tab", "\t")[0] : throw new ArgumentOutOfRangeException());
            set => InternalFieldSeparator = value.ToString().Replace("\t", "Tab");
        }

        [XmlAttribute("record-separator")]
        [DefaultValue("CrLf")]
        public string InternalRecordSeparator { get; set; }

        [XmlIgnore]
        public string RecordSeparator 
        {
            get => InternalRecordSeparator.Replace("Cr", "\r").Replace("Lf", "\n");
            set => InternalRecordSeparator = value.Replace("\r", "Cr").Replace("\n", "Lf");
        }

        [XmlAttribute("first-row-header")]
        [DefaultValue(false)]
        public bool InternalFirstRowHeader { get; set; }

        [XmlIgnore]
        public bool FirstRowHeader
        {
            get => InternalFirstRowHeader;
            set => InternalFirstRowHeader = value;
        }

        [XmlAttribute("empty-cell")]
        [DefaultValue("(empty)")]
        public string InternalEmptyCell { get; set; }

        [XmlIgnore]
        public string EmptyCell
        {
            get => string.IsNullOrEmpty(InternalEmptyCell) ? "(empty)" : InternalEmptyCell;
            set => InternalEmptyCell = value;
        }

        [XmlAttribute("missing-cell")]
        [DefaultValue("(null)")]
        public string InternalMissingCell { get; set; }

        [XmlIgnore]
        public string MissingCell
        {
            get => string.IsNullOrEmpty(InternalMissingCell) ? "(null)" : InternalMissingCell;
            set => InternalMissingCell = value;
        }
    }
}
