using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Interval;

public class DateTimeIntervalBuilder
{
    private readonly string? value;
    private bool isBuild;
    protected DateTimeInterval? interval;
    protected Exception? ex;

    public DateTimeIntervalBuilder(string value)
    {
        this.value = value;
        isBuild = false;
    }

    public DateTimeIntervalBuilder(object value)
    {
        if (value is string str)
            this.value = str;
        else
            ex = new ArgumentException("This must be a string");

        isBuild = false;
    }

    public void Build()
    {
        if (ex == null)
        {
            var valueToBuild = value!.Replace(" ", "").ToLower();

            interval = BuildClassic(valueToBuild) ?? throw new NullReferenceException();
        }
        isBuild = true;
    }

    protected virtual DateTimeInterval? BuildClassic(string value)
    {
        if (!(value.StartsWith("]") || value.StartsWith("[")))
            ex = new ArgumentException("The interval definition must start by '[' or ']'");
        if (!(value.EndsWith("]") || value.EndsWith("[")))
            ex = new ArgumentException("The interval definition must end by '[' or ']'");
        if (!(value.Contains(';')))
            ex = new ArgumentException("The interval definition must contain a delimitor ';'");

        var split = value.Split(';');
        if (split.Length > 2)
            ex = new ArgumentException("The interval definition must contain only one delimitor ';'");

        if (ex != null)
            return null;

        EndPoint<DateTime> left, right;
        if (split[0].StartsWith("["))
            left = new LeftEndPointClosed<DateTime>(
                    DateTime.Parse(split[0][1..], CultureInfo.InvariantCulture.DateTimeFormat));
        else
            left = new LeftEndPointOpen<DateTime>(
                    DateTime.Parse(split[0][1..], CultureInfo.InvariantCulture.DateTimeFormat));

        if (split[1].EndsWith("]"))
            right = new RightEndPointClosed<DateTime>(
                    DateTime.Parse(split[1][..^1], CultureInfo.InvariantCulture.DateTimeFormat));
        else
            right = new RightEndPointOpen<DateTime>(
                    DateTime.Parse(split[1][..^1], CultureInfo.InvariantCulture.DateTimeFormat));

        return new DateTimeInterval(left, right);
    }

    public bool IsValid()
    {
        if (!isBuild)
            throw new InvalidOperationException("You must first apply the build method before a call to this method.");

        return interval != null;
    }

    public DateTimeInterval? GetInterval()
    {
        if (!isBuild)
            throw new InvalidOperationException("You must first apply the build method before a call to this method.");

        return interval;
    }

    public Exception? GetException()
    {
        if (!isBuild)
            throw new InvalidOperationException("You must first apply the build method before a call to this method.");

        return ex;
    }
}
