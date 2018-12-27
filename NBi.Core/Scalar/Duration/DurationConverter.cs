using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Duration
{
    public class DurationConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType==typeof(string);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string))
                throw new ArgumentOutOfRangeException();
            if (value == null)
                throw new ArgumentNullException();

            var str = (value as string).Trim().ToLowerInvariant();
            if (str.EndsWith("month") || str.EndsWith("months"))
                return new MonthDuration(ParseValue(str));
            if (str.EndsWith("year") || str.EndsWith("years"))
                return new YearDuration(ParseValue(str));

            var ts = TimeSpan.Parse(str, culture.DateTimeFormat);
            return new FixedDuration(ts);
        }

        private int ParseValue(string value) => int.Parse(string.Concat(value.TakeWhile(c => char.IsDigit(c))));

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            switch (value)
            {
                case MonthDuration x: return $"{x.Count} month{(x.Count>1 ? "s" : string.Empty)}";
                case YearDuration x: return $"{x.Count} year{(x.Count > 1 ? "s" : string.Empty)}";
                case FixedDuration x: return x.TimeSpan.ToString();
                default:
                    throw new ArgumentException();
            }
        }
    }
}
