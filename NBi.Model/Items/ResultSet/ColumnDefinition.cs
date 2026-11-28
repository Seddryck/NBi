using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Transformation;
using NBi.Extensibility;
using NBi.Model.Items.Alteration.Transform;

namespace NBi.Model.Items.ResultSet;

public class ColumnDefinitionLight : IColumnDefinitionLight
{
    public string IdentifierSerializer { get; set; }
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }
    public ColumnType Type { get; set; }

    public ColumnDefinitionLight()
    {
        Type = ColumnType.Text;
    }
}

public class ColumnDefinition: IColumnDefinition
{
    public int Index {get; set;}
    public string Name { get; set; }
    public ColumnRole Role{get; set;}
    public ColumnType Type{get; set;}

    public bool IsToleranceSpecified
    {
        get => !string.IsNullOrEmpty(Tolerance);
    }

    public bool IndexSpecified
    {
        get => string.IsNullOrEmpty(Name);
    }

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

    public string Tolerance { get; set; }

    public Rounding.RoundingStyle RoundingStyle {get; set;}

    public string RoundingStep {get; set;}

    public LightTransform TransformationInner { get; set; }

    public LightTransform InternalTransformationInner { get => TransformationInner; set => TransformationInner=value; }

    public ITransformationInfo Transformation
    {
        get { return TransformationInner; }
    }
}
