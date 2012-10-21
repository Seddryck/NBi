using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Systems.Structure
{
    public class MeasureXml : MeasureGroupXml
    {
        public override bool IsStructure()
        {
            return true;
        }

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
            var cmd = DiscoveryFactory.BuildForMeasureGroup(ConnectionString ?? Default.ConnectionString, Perspective, MeasureGroup);
            return cmd;
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
