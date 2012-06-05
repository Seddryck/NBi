using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class ContainsXml : AbstractConstraintXml
    {
        [XmlAttribute("ignore-case")]
        public bool IgnoreCase { get; set; }

        [XmlAttribute("caption")]
        public string Caption { get; set; }

        protected string _displayFolder; 

        [XmlAttribute("display-folder")]
        public string DisplayFolder
        {
            get
            { return _displayFolder; }

            set
            {
                _displayFolder = value;
                Specification.IsDisplayFolderSpecified = true;
            }
        }

        public ContainsXml()
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
