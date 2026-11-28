using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Alteration.Summarization;

public abstract class Aggregation
{
    public AggregationFunctionType Function { get; set; }
    public Aggregation(AggregationFunctionType function) => Function = function;

    public ColumnType ColumnType { get; set; }

    public string EmptySeriesStrategyName { get; set; }

    public string MissingValuesStrategyName { get; set; }

    public virtual IEnumerable<string> Parameters => Array.Empty<string>();
}

public abstract class ColumnAggregation : Aggregation
{
    public ColumnAggregation(AggregationFunctionType function)
        : base(function) { }

    public string IdentifierSerializer { get; set; }
    public IColumnIdentifier Identifier
    {
        get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
        set => IdentifierSerializer = value.Label;
    }
}

public class Sum : ColumnAggregation
{
    public Sum()
        : base(AggregationFunctionType.Sum) { }
}

public class Average : ColumnAggregation
{
    public Average()
        : base(AggregationFunctionType.Average) { }
}

public class Max : ColumnAggregation
{
    public Max()
        : base(AggregationFunctionType.Max) { }
}

public class Min : ColumnAggregation
{
    public Min()
        : base(AggregationFunctionType.Min) { }
}

public class CountRows : Aggregation
{
    public CountRows()
        : base(AggregationFunctionType.Count) { }
}

public class Concatenation : ColumnAggregation
{
    public Concatenation()
        : base(AggregationFunctionType.Concatenation) { }

    public string Separator { get; set; }

    public override IEnumerable<string> Parameters => new List<string>() { Separator };
}
