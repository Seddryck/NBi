using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Comparer
{
    public class TextSingleMethodTolerance : TextTolerance
    {
        public string Style { get; private set; }
        public double Value { get; private set; }

        public Func<string, string, double> Implementation { get; private set; }
        public Func<double, double, bool> Predicate { get; private set; }

        public TextSingleMethodTolerance(string style, double value, Func<string, string, double> func, Func<double, double, bool> predicate)
            : base($"{style} ({value.ToString(NumberFormatInfo.InvariantInfo)})")
        {
            Style = style;
            Value = value;
            Implementation = func;
            Predicate = predicate;
        }
    }
}
