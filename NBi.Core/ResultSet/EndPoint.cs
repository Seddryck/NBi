using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public abstract class EndPoint
    {
        public double Value { get; set; }
        public bool IsClosed { get; set; }
        public bool IsOpen
        {
            get
            {
                return !IsClosed;
            }
        }

        public EndPoint(double value, bool isClosed)
        {
            Value = value;
            IsClosed = isClosed;
        }

        protected abstract string BoundSymbol { get; }
    }

    public abstract class LeftEndPoint : EndPoint
    {
        public LeftEndPoint(double value, bool isClosed)
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

    public abstract class RightEndPoint : EndPoint
    {
        public RightEndPoint(double value, bool isClosed)
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

    public class LeftEndPointClosed : LeftEndPoint
    {
        public LeftEndPointClosed(double value)
            : base(value, true)
        {
        }
    }

    public class LeftEndPointOpen : LeftEndPoint
    {
        public LeftEndPointOpen(double value)
            : base(value, false)
        {
        }
    }

    public class RightEndPointClosed : RightEndPoint
    {
        public RightEndPointClosed(double value)
            : base(value, true)
        {
        }
    }

    public class RightEndPointOpen : RightEndPoint
    {
        public RightEndPointOpen(double value)
            : base(value, false)
        {
        }
    }

    public class LeftEndPointNegativeInfinity : LeftEndPoint
    {
        public LeftEndPointNegativeInfinity()
            : base(double.NegativeInfinity, true)
        {
        }
    }

    public class RightEndPointPositiveInfinity : RightEndPoint
    {
        public RightEndPointPositiveInfinity()
            : base(double.PositiveInfinity, true)
        {
        }
    }
}
