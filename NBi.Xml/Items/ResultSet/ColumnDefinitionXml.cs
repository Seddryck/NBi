using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Transformation;
using NBi.Extensibility;
using NBi.Xml.Items.Alteration.Transform;

namespace NBi.Xml.Items.ResultSet;

public class ColumnDefinitionLightXml : IColumnDefinitionLight
{
    [XmlAttribute("identifier")]
    public string IdentifierSerializer { get; set; }
    [XmlIgnore]
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }
    [XmlAttribute("type")]
    [DefaultValue(ColumnType.Text)]
    public ColumnType Type { get; set; }

    public ColumnDefinitionLightXml()
    {
        Type = ColumnType.Text;
    }
}

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
        get => !string.IsNullOrEmpty(Tolerance);
    }

    [XmlIgnore()]
    public bool IndexSpecified
    {
        get => string.IsNullOrEmpty(Name);
    }

    [XmlIgnore]
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(string.IsNullOrEmpty(Name) ? $"#{Index}" : Name);
        set
        {
            if (value.Label.StartsWith("#"))
                Index = int.Parse(value.Label.Substring(1));
            else
                Name = value.Label;
        }
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

    [XmlElement("transform")]
    public LightTransformXml TransformationInner { get; set; }

    [XmlIgnore]
    public LightTransformXml InternalTransformationInner { get => TransformationInner; set => TransformationInner=value; }

    [XmlIgnore]
    public ITransformationInfo Transformation
    {
        get { return TransformationInner; }
    }
}
