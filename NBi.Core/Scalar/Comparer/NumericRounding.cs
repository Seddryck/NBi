using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Comparer;

class NumericRounding : Rounding
{
    protected decimal step;

    public NumericRounding(decimal step, Rounding.RoundingStyle style)
        : base(step.ToString(NumberFormatInfo.InvariantInfo), style)
    {
        if (step <= 0)
            throw new ArgumentException("The parameter '{0}' must be a value greater than zero.", nameof(step));

        this.step = step;
    }

    public decimal GetValue(decimal value)
    {
        return GetValue(value, step);
    }

}
