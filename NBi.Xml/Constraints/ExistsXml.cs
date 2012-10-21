using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class ExistsXml : AbstractConstraintXml
    {
        [XmlAttribute("ignore-case")]
        public bool IgnoreCase { get; set; }

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

        public ExistsXml()
        {
            Specification = new SpecificationContains();
        }

        [XmlIgnore()]
        public SpecificationContains Specification { get; protected set; }

        public class SpecificationContains
        {
            public bool IsDisplayFolderSpecified { get; internal set; }
        }
    }
}
