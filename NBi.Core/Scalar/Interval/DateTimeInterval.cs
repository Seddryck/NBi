using System;
using System.Linq;

namespace NBi.Core.Scalar.Interval;

	public class DateTimeInterval : BaseInterval<DateTime>
	{
		public DateTimeInterval(EndPoint<DateTime> left, EndPoint<DateTime> right)
        : base(left, right)
		{ }


		public override bool Contains(DateTime value)
		{
			if (value<Left.Value || value>Right.Value)
				return false;

			if (value==Left.Value && Left.IsOpen)
				return false;

			if (value == Right.Value && Right.IsOpen)
				return false;

			return true;
		}

    public override string ToString()
    {
        if (Left.Value.TimeOfDay.Ticks==0 && Right.Value.TimeOfDay.Ticks == 0)
            return $"{Left.BoundSymbol}{Left.Value:yyyy-MM-dd};{Right.Value:yyyy-MM-dd}{Right.BoundSymbol}";
        else
            return $"{Left.BoundSymbol}{Left.Value:yyyy-MM-dd HH:mm:ss};{Right.Value:yyyy-MM-dd HH:mm:ss}{Right.BoundSymbol}";
    }

}
