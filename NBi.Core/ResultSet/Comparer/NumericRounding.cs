using System;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class NumericRounding : Rounding
    {
        protected double step;

        public NumericRounding(double step, Rounding.RoundingStyle style)
            : base(step.ToString(), style)
        {
            if (step <= 0)
                throw new ArgumentException("The parameter '{0}' must be a value greater than zero.", "step");

            this.step = step;
        }

        public decimal GetValue(decimal value)
        {
            return Convert.ToDecimal(GetValue(Convert.ToDouble(value), step));
        }

        
    }
}
