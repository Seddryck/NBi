using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
    public class TextTolerance : Tolerance
    {
        public string Style { get; private set; }
        public Double Value { get; private set; }

        public Func<string, string, double> Implementation { get; private set; }
        public Func<double, double, bool> Predicate { get; private set; }

        public TextTolerance(string style, double value, Func<string, string, double> func, Func<double, double, bool> predicate)
            : base($"{style} ({value.ToString(NumberFormatInfo.InvariantInfo)})")
        {
            Style = style;
            Value = value;
            Implementation = func;
            Predicate = predicate;
        }

        public static TextTolerance None
        {
            get
            {
                if (none==null)
                    none = new TextTolerance("None", 1, (x, y) => x == y ? 1 : 0, (x, y) => x == 1);
                return none;
            }
        }

        private static TextTolerance none;
    }
}
