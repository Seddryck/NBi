using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Formatter
{
    class CellFormatter
    {

        public CellFormatter()
        {
        }

        public string GetDisplay(object obj)
        {
            if (obj is DBNull)
                return "(null)";
            if (obj is string && ((string)obj).Length == 0)
                return "(empty)";
            if (obj is string)
                return obj.ToString();

            Double dbl = 0;
            if (Double.TryParse(obj.ToString(), out dbl))
                return dbl.ToString("G", CultureInfo.InvariantCulture);

            Decimal dec = 0;
            if (Decimal.TryParse(obj.ToString(), out dec))
                return dec.ToString("G", CultureInfo.InvariantCulture);

            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParse(obj.ToString(), out dt))
                return dt.ToString(DateTimeFormatInfo.InvariantInfo.UniversalSortableDateTimePattern).Substring(0, 19);

            return obj.ToString();
        }


        public IEnumerable<string> Tabulize(object obj, int totalWidth)
        {
            return new List<string>
            {
                this.GetDisplay(obj).PadRight(totalWidth)
            };
        }
    }
}
