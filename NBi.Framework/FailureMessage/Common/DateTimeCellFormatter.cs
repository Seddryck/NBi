using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Unit.Framework.FailureMessage.Common
{
    public class DateTimeCellFormatter : CellFormatter
    {
        protected override string FormatNotNull(object value)
        {
            var typeName = value.GetType().Name.ToLower();
            switch (typeName)
            {
                case "datetime": return FormatDateTime((DateTime)value);
                case "string": return FormatString((string)value);
                default:
                    return FormatString(value.ToString());
            }
        }

        protected string FormatDateTime(DateTime value)
        {
            if (value.TimeOfDay.Ticks == 0)
                return value.ToString("yyyy-MM-dd");
            else if (value.Millisecond == 0)
                return value.ToString("yyyy-MM-dd hh:mm:ss");
            else
                return value.ToString("yyyy-MM-dd hh:mm:ss.fff");
        }

        
        protected string FormatString(string value)
        {
            DateTime valueDateTime = DateTime.MinValue;
            if (DateTime.TryParse(value, out valueDateTime))
                return FormatDateTime(valueDateTime);
            else
                return value;
        }
    }
}
