using System;
using System.Linq;

namespace NBi.Core.ResultSet
{
	public class Interval
	{
		public EndPoint Left { get; set; }
		public EndPoint Right { get; set; }
        
		public Interval(EndPoint left, EndPoint right)
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
	}
}
