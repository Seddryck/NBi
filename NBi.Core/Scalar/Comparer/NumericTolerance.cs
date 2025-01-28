using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Comparer;

public abstract class NumericTolerance : Tolerance
{
    public decimal Value { get; set; }
    public SideTolerance Side { get; private set; }

    public NumericTolerance(decimal value, SideTolerance side)
        : base(value.ToString(NumberFormatInfo.InvariantInfo))
    {
        Side = side;
        Value = value;
    }
    
}
