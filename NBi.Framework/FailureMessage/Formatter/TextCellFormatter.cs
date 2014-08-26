using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Unit.Framework.FailureMessage.Formatter
{
    public class TextCellFormatter : CellFormatter
    {
        protected override string FormatNotNull(object value)
        {
            var typeName = value.GetType().Name.ToLower();
            switch (typeName)
            {
                case "string": return FormatString((string)value);
                default:
                    return FormatString(value.ToString());
            }
        }

        protected string FormatString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "(empty)";
            else if (string.IsNullOrEmpty(value.Trim()))
                return string.Format("({0} space{1})", value.Length, value.Length>1 ? "s" : string.Empty);
            else
                return value;
        }
    }
}
