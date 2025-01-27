using System;
using System.Linq;

namespace NBi.Core.Scalar.Interval;

	public class NumericInterval : BaseInterval<double>
	{
    public NumericInterval(EndPoint<double> left, EndPoint<double> right)
        : base(left, right)
    { }


		public override bool Contains(double value)
		{
			if (value<Left.Value || value>Right.Value)
				return false;

			if (value==Left.Value && Left.IsOpen)
				return false;

			if (value == Right.Value && Right.IsOpen)
				return false;

			return true;
		}

    public bool Contains(decimal value)
    {
        return Contains(Convert.ToDouble(value));
    }
}
