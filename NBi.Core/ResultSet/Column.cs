using System;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Transformation;
using NBi.Extensibility;

namespace NBi.Core.ResultSet
{
    public class Column : IColumnDefinition
    {
        public IColumnIdentifier Identifier { get; set; }
        public ColumnRole Role { get; set; }
        public ColumnType Type { get; set; }
        public string Tolerance { get; set; } = string.Empty;
        public Rounding.RoundingStyle RoundingStyle { get; set; } = Rounding.RoundingStyle.None;
        public string RoundingStep { get; set; } = string.Empty;
        public ITransformationInfo? Transformation { get; set; }

        public Column(IColumnIdentifier identifier, ColumnRole role, ColumnType type, string tolerance)
            => (Identifier, Role, Type, Tolerance) = (identifier, role, type, tolerance);

        public Column(IColumnIdentifier identifier, ColumnRole role, ColumnType type)
            : this(identifier, role, type, string.Empty) { }

        public bool IsToleranceSpecified
        {
            get { return !string.IsNullOrEmpty(Tolerance); }
        }
    }
}
