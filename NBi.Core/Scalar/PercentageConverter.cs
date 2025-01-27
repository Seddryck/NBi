using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar;

public class PercentageConverter : TypeConverter
{
    static TypeConverter conv = TypeDescriptor.GetConverter(typeof(double));

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return conv.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        if (destinationType == typeof(Percentage))
        {
            return true;
        }

        return conv.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value == null)
            return null;
        culture ??= CultureInfo.InvariantCulture;

        if (value is string s)
        {
            s = s.TrimEnd(' ', '\t', '\r', '\n');

            var percentage = s.EndsWith(culture.NumberFormat.PercentSymbol);
            if (percentage)
                s = s[..^culture.NumberFormat.PercentSymbol.Length];

            double result = (double)(conv.ConvertFromString(context, culture, s) ?? throw new NotSupportedException());
            if (percentage)
                result /= 100;

            return new Percentage(result);
        }

        return new Percentage((double)(conv.ConvertFrom(context, culture, value) ?? throw new NotSupportedException()));
    }

    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is not Percentage)
            throw new ArgumentNullException(nameof(value));

        var pct = (Percentage)value;

        culture ??= CultureInfo.InvariantCulture;

        if (destinationType == typeof(string))
            return conv.ConvertTo(context, culture, pct.Value * 100, destinationType) + culture.NumberFormat.PercentSymbol;

        return conv.ConvertTo(context, culture, pct.Value, destinationType)!;
    }

}
