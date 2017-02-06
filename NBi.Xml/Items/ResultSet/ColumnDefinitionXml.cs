using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Core.Transformation;

namespace NBi.Xml.Items.ResultSet
{
    public class ColumnDefinitionXml: IColumnDefinition
    {
        [XmlAttribute("index")]
        public int Index {get; set;}
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("role")]
        public ColumnRole Role{get; set;}
        [XmlAttribute("type")]
        public ColumnType Type{get; set;}

        [XmlIgnore()]
        public bool IsToleranceSpecified
        {
            get { return !string.IsNullOrEmpty(Tolerance); }
        }

        [XmlAttribute("tolerance")]
        [DefaultValue("")]
        public string Tolerance { get; set; }

        [XmlAttribute("rounding-style")]
        [DefaultValue(Rounding.RoundingStyle.None)]
        public Rounding.RoundingStyle RoundingStyle {get; set;}
        
        [XmlAttribute("rounding-step")]
        [DefaultValue("")]
        public string RoundingStep {get; set;}

        [XmlElement("transformation")]
        public TransformationXml TransformationInner { get; set; }

        [XmlIgnore]
        public ITransformationInfo Transformation
        {
            get { return TransformationInner; }
        }
    }
}
