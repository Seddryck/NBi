using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class MeasureXml : MeasureGroupXml
    {
        protected string measureGroup;
        [XmlAttribute("measure-group")]
        public string MeasureGroup
        {
            get
            { return measureGroup; }

            set
            {
                measureGroup = value;
                Specification.IsMeasureGroupSpecified = true;
            }
        }

        protected string displayFolder;
        [XmlAttribute("display-folder")]
        public string DisplayFolder
        {
            get
            { return displayFolder; }

            set
            {
                displayFolder = value;
                Specification.IsDisplayFolderSpecified = true;
            }
        }

        public MeasureXml()
        {
            Specification = new SpecificationMeasure();
        }

        public override object Instantiate()
        {
            //TODO here?
            return null;
        }

        [XmlIgnore()]
        public SpecificationMeasure Specification { get; protected set; }

        public class SpecificationMeasure
        {
            public bool IsDisplayFolderSpecified { get; internal set; }
            public bool IsMeasureGroupSpecified { get; internal set; }
        }

    }
}
