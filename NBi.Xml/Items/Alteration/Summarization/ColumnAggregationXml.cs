using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Summarization;

public abstract class AggregationXml
{
    [XmlIgnore]
    public AggregationFunctionType Function { get; set; }
    public AggregationXml(AggregationFunctionType function) => Function = function;
    
    [XmlAttribute("type")]
    public ColumnType ColumnType { get; set; }

    [XmlAttribute("if-empty")]
    public string EmptySeriesStrategyName { get; set; }

    [XmlAttribute("if-missing-value")]
    public string MissingValuesStrategyName { get; set; }

    [XmlIgnore]
    public virtual IEnumerable<string> Parameters => Array.Empty<string>();
}

public abstract class ColumnAggregationXml : AggregationXml
{
    public ColumnAggregationXml(AggregationFunctionType function) : base(function) { }

    [XmlAttribute("column")]
    public string IdentifierSerializer { get; set; }
    [XmlIgnore]
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }
}

public class SumXml : ColumnAggregationXml
{
    public SumXml() : base(AggregationFunctionType.Sum) { }
}

public class AverageXml : ColumnAggregationXml
{
    public AverageXml() : base(AggregationFunctionType.Average) { }
}

public class MaxXml : ColumnAggregationXml
{
    public MaxXml() : base(AggregationFunctionType.Max) { }
}

public class MinXml : ColumnAggregationXml
{
    public MinXml() : base(AggregationFunctionType.Min) { }
}

public class CountRowsXml : AggregationXml
{
    public CountRowsXml() : base(AggregationFunctionType.Count) { }
}

public class ConcatenationXml : ColumnAggregationXml
{
    public ConcatenationXml() : base(AggregationFunctionType.Concatenation) { }

    [XmlAttribute("separator")]
    public string Separator { get; set; }

    [XmlIgnore]
    public override IEnumerable<string> Parameters => new List<string>() { Separator };
}
