using System.Xml.Serialization;
using NBi.Core.ResultSet;

namespace NBi.Xml.Constraints.EqualTo
{
    public class ColumnXml: IColumn
    {
        [XmlAttribute("index")]
        public int Index {get; set;}
        [XmlAttribute("role")]
        public ColumnRole Role{get; set;}
        [XmlAttribute("type")]
        public ColumnType Type{get; set;}

        protected bool _isToleranceSpecified;
        [XmlIgnore()]
        public bool IsToleranceSpecified
        {
            get { return _isToleranceSpecified; }
            set { _isToleranceSpecified = value; }
        }

        protected decimal _tolerance;
        [XmlAttribute("tolerance")]
        public decimal Tolerance
        {
            get
            { return _tolerance; }

            set
            {
                _tolerance = value;
                _isToleranceSpecified = true;
            }
        }
    }
}
