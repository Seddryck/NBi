using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public class EndPoint
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

    }

    public class EndPointClosed : EndPoint
    {
        public EndPointClosed(double value)
            : base(value, true)
        {
        }
    }

    public class EndPointOpen : EndPoint
    {
        public EndPointOpen(double value)
            : base(value, false)
        {
        }
    }

    public class LeftEndPointNegativeInfinity : EndPoint
    {
        public LeftEndPointNegativeInfinity()
            : base(double.NegativeInfinity, true)
        {
        }
    }

    public class RightEndPointPositiveInfinity : EndPoint
    {
        public RightEndPointPositiveInfinity()
            : base(double.PositiveInfinity, true)
        {
        }
    }
}
