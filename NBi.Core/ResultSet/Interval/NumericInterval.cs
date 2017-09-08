using System;
using System.Linq;

namespace NBi.Core.ResultSet.Interval
{
	public class NumericInterval
	{
		public EndPoint<double> Left { get; set; }
		public EndPoint<double> Right { get; set; }
		
		public NumericInterval(EndPoint<double> left, EndPoint<double> right)
		{
			Left = left;
			Right = right;
		}


		public bool Contains(double value)
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

        public override string ToString()
        {
            return string.Format("{0};{1}", Left.ToString(), Right.ToString());
        }
	}
}
