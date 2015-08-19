using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    public abstract class NumericTolerance : Tolerance
    {
        public decimal Value { get; set; }

        public NumericTolerance(decimal value, SideTolerance side)
            : base(value.ToString(NumberFormatInfo.InvariantInfo), side)
        {
            Value = value;
        }
        
    }
}
