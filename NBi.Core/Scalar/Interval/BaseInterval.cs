using System;
using System.Linq;

namespace NBi.Core.Scalar.Interval;

	public abstract class BaseInterval<T>
	{
		public EndPoint<T> Left { get; set; }
		public EndPoint<T> Right { get; set; }
		
		public BaseInterval(EndPoint<T> left, EndPoint<T> right)
		{
			Left = left;
			Right = right;
		}


    public abstract bool Contains(T value);
    
    public override string ToString()
			=> $"{Left};{Right}";
	}
