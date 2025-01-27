using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Interval;

public abstract class EndPoint<T>(T value, bool isClosed)
{
    public T Value { get; set; } = value;
    public bool IsClosed { get; set; } = isClosed;
    public bool IsOpen
        => !IsClosed;

    public abstract string BoundSymbol { get; }
}

public abstract class LeftEndPoint<T>(T value, bool isClosed) : EndPoint<T>(value, isClosed)
{
    public override string BoundSymbol
        => IsClosed ? "[" : "]";

    public override string ToString()
        => $"{BoundSymbol}{Value}";
}

public abstract class RightEndPoint<T>(T value, bool isClosed) : EndPoint<T>(value, isClosed)
{
    public override string BoundSymbol
        => IsClosed ? "]" : "[";

    public override string ToString()
        => $"{Value}{BoundSymbol}";
}

public class LeftEndPointClosed<T>(T value) : LeftEndPoint<T>(value, true)
{ }

public class LeftEndPointOpen<T>(T value) : LeftEndPoint<T>(value, false)
{ }

public class RightEndPointClosed<T>(T value) : RightEndPoint<T>(value, true)
{ }

public class RightEndPointOpen<T>(T value) : RightEndPoint<T>(value, false)
{ }

public class LeftEndPointNegativeInfinity : LeftEndPoint<double>
{
    public LeftEndPointNegativeInfinity()
        : base(double.NegativeInfinity, true)
    { }
}

public class RightEndPointPositiveInfinity : RightEndPoint<double>
{
    public RightEndPointPositiveInfinity()
        : base(double.PositiveInfinity, true)
    { }
}
