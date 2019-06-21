using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Sequence.Transformation
{
    public abstract class AggregationXml
    {
        [XmlIgnore]
        public AggregationFunctionType Function { get; set; }
        public AggregationXml(AggregationFunctionType function) => Function = Function;

        [XmlAttribute("column")]
        public string IdentifierSerializer { get; set; }
        [XmlIgnore]
        public IColumnIdentifier Identifier
        {
            get => new ColumnIdentifierFactory().Instantiate(IdentifierSerializer);
            set => IdentifierSerializer = value.Label;
        }
        [XmlAttribute("type")]
        public ColumnType ColumnType { get; set; }
        [XmlAttribute("if-empty")]
        public string EmptySeriesStrategyName { get; set; }
        [XmlAttribute("if-missing-value")]
        public string MissingValuesStrategyName { get; set; }
    }

    public class SumXml : AggregationXml
    {
        public SumXml() : base(AggregationFunctionType.Sum) { } 
    }

    public class MeanXml : AggregationXml
    {
        public MeanXml() : base(AggregationFunctionType.Mean) { }
    }

    public class MaxXml : AggregationXml
    {
        public MaxXml() : base(AggregationFunctionType.Max) { }
    }

    public class MinXml : AggregationXml
    {
        public MinXml() : base(AggregationFunctionType.Min) { }
    }
}