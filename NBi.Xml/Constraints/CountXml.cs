using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class CountXml : AbstractConstraintXml
    {
        
        protected int _exactly; 
        [XmlAttribute("exactly")]
        public int Exactly 
        { 
            get 
            { return _exactly; } 
            
            set 
            {
                _exactly = value;
                Specification.IsExactlySpecified = true;
            }
        }

        protected int _moreThan; 
        [XmlAttribute("more-Than")]
        public int MoreThan
        {
            get
            { return _moreThan; }

            set
            {
                _moreThan = value;
                Specification.IsMoreThanSpecified = true;
            }
        }

        protected int _lessThan;
        [XmlAttribute("less-Than")]
        public int LessThan
        {
            get
            { return _lessThan; }

            set
            {
                _lessThan = value;
                Specification.IsLessThanSpecified = true;
            }
        }

        public CountXml()
        {
            Specification = new SpecificationCount();
        }

        [XmlIgnore()]
        public SpecificationCount Specification { get; protected set; }


        public class SpecificationCount
        {
            public bool IsExactlySpecified { get; internal set; }
            public bool IsMoreThanSpecified { get; internal set; }
            public bool IsLessThanSpecified { get; internal set; }
        }
    }
}
