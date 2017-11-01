using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Unit.Framework.FailureMessage.Common
{
    public abstract class CellFormatter
    {
        public string Format(object value)
        {
            if (value == null || value is DBNull)
                return FormatNull(value);
            else if (value is string && (string)value=="(null)")
                return FormatNull(value);
            else
                return FormatNotNull(value);
        }

        protected virtual string FormatNull(object value)
        {
            return "(null)";
        }

        protected virtual string FormatNotNull(object value)
        {
            return value.ToString();
        }

    }
}
