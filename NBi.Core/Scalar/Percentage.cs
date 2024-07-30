using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar
{
    [TypeConverter(typeof(PercentageConverter))]
    public class Percentage
    {
        public double Value { get; }

        public Percentage(double value)
        {
            Value = value;
        }

        public Percentage(string value)
        {
            var pct = (Percentage)(TypeDescriptor.GetConverter(GetType()).ConvertFromString(value) ?? throw new NullReferenceException());
            Value = pct.Value;
        }

        public override string ToString()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        public string ToString(CultureInfo Culture)
        {
            return TypeDescriptor.GetConverter(GetType()).ConvertToString(null, Culture, this) ?? string.Empty;
        }
    }
}
