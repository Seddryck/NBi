using System.ComponentModel;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class ContainsXml : AbstractConstraintXml
    {
        [XmlAttribute("ignore-case")]
        [DefaultValue(false)]
        public bool IgnoreCase { get; set; }

        [XmlAttribute("caption")]
        public string Caption { get; set; }


        public ContainsXml()
        {
            Specification = new SpecificationContains();
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

        [XmlIgnore()]
        public SpecificationContains Specification { get; protected set; }


        public class SpecificationContains
        {
            public bool IsDisplayFolderSpecified { get; internal set; }
        }
    }
}
