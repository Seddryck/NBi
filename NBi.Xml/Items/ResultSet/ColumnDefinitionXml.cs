using System.Xml.Serialization;
using NBi.Core.ResultSet;

namespace NBi.Xml.Items.ResultSet
{
    public class ColumnDefinitionXml: IColumnDefinition
    {
        [XmlAttribute("index")]
        public int Index {get; set;}
        [XmlAttribute("role")]
        public ColumnRole Role{get; set;}
        [XmlAttribute("type")]
        public ColumnType Type{get; set;}

        protected bool isToleranceSpecified;
        [XmlIgnore()]
        public bool IsToleranceSpecified
        {
            get { return isToleranceSpecified; }
            set { isToleranceSpecified = value; }
        }

        protected decimal tolerance;
        [XmlAttribute("tolerance")]
        public decimal Tolerance
        {
            get
            { return tolerance; }

            set
            {
                tolerance = value;
                isToleranceSpecified = true;
            }
        }
    }
}
