using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Interval
{
    public abstract class EndPoint<T>
    {
        public T Value { get; set; }
        public bool IsClosed { get; set; }
        public bool IsOpen
        {
            get
            {
                return !IsClosed;
            }
        }

        public EndPoint(T value, bool isClosed)
        {
            Value = value;
            IsClosed = isClosed;
        }

        protected abstract string BoundSymbol { get; }
    }

    public abstract class LeftEndPoint<T> : EndPoint<T>
    {
        public LeftEndPoint(T value, bool isClosed)
            : base(value, isClosed)
        {
        }

        protected override string BoundSymbol
        {
            get
            {
                return IsClosed ? "[" : "]";
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", BoundSymbol, Value.ToString());
        }
    }

    public abstract class RightEndPoint<T> : EndPoint<T>
    {
        public RightEndPoint(T value, bool isClosed)
            : base(value, isClosed)
        {
        }

        protected override string BoundSymbol
        {
            get
            {
                return IsClosed ? "]" : "[";
            }
        }

        public override string ToString()
        {
            return string.Format("{1}{0}", BoundSymbol, Value.ToString());
        }
    }

    public class LeftEndPointClosed<T> : LeftEndPoint<T>
    {
        public LeftEndPointClosed(T value)
            : base(value, true)
        {
        }
    }

    public class LeftEndPointOpen<T> : LeftEndPoint<T>
    {
        public LeftEndPointOpen(T value)
            : base(value, false)
        {
        }
    }

    public class RightEndPointClosed<T> : RightEndPoint<T>
    {
        public RightEndPointClosed(T value)
            : base(value, true)
        {
        }
    }

    public class RightEndPointOpen<T> : RightEndPoint<T>
    {
        public RightEndPointOpen(T value)
            : base(value, false)
        {
        }
    }

    public class LeftEndPointNegativeInfinity : LeftEndPoint<double>
    {
        public LeftEndPointNegativeInfinity()
            : base(double.NegativeInfinity, true)
        {
        }
    }

    public class RightEndPointPositiveInfinity : RightEndPoint<double>
    {
        public RightEndPointPositiveInfinity()
            : base(double.PositiveInfinity, true)
        {
        }
    }
}
