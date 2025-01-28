using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Duration;

public class DurationConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType==typeof(string);

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(string);

    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value is not string)
            throw new ArgumentOutOfRangeException();

        var str = (value as string)?.Trim()?.ToLowerInvariant() ?? throw new ArgumentNullException();
        if (str.EndsWith("month") || str.EndsWith("months"))
            return new MonthDuration(DurationConverter.ParseValue(str));
        if (str.EndsWith("year") || str.EndsWith("years"))
            return new YearDuration(DurationConverter.ParseValue(str));

        if (str.EndsWith("day") || str.EndsWith("days"))
            return new FixedDuration(new TimeSpan(DurationConverter.ParseValue(str),0,0,0));

        var ts = TimeSpan.Parse(str, (culture ?? CultureInfo.InvariantCulture).DateTimeFormat);
        return new FixedDuration(ts);
    }

    private static int ParseValue(string value) 
        => int.Parse(string.Concat(value.TakeWhile(c => char.IsDigit(c))));

    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value switch
        {
            MonthDuration x => $"{x.Count} month{(x.Count > 1 ? "s" : string.Empty)}",
            YearDuration x => $"{x.Count} year{(x.Count > 1 ? "s" : string.Empty)}",
            FixedDuration x => x.TimeSpan.ToString(),
            _ => throw new ArgumentException(),
        };
    }
}
