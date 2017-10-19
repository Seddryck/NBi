using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Unit.Framework.FailureMessage.Common
{
    public class BooleanCellFormatter : CellFormatter
    {
        protected override string FormatNotNull(object value)
        {
            var typeName = value.GetType().Name.ToLower();
            switch (typeName)
            {
                case "boolean": return FormatBoolean((bool)value);
                case "string": return FormatString((string)value);
                case "decimal": return FormatBooleanObject((decimal)value);
                case "short": return FormatBooleanObject(value);
                case "int": return FormatBooleanObject(value);
                case "double": return FormatBooleanObject(value);
                case "float": return FormatBooleanObject(value);
                default:
                    return FormatString(value.ToString());
            }
        }

        protected string FormatBoolean(bool value)
        {
            if (value)
                return "True";
            else
                return "False";
        }

        protected string FormatBooleanObject(object value)
        {
            return FormatBoolean(Convert.ToBoolean(value));
        }

        protected string FormatString(string value)
        {
            var workingValue = value.ToLower().Trim();
            if (new[] { "true", "yes", "1" }.Contains(workingValue))
                return FormatBoolean(true);
            else if (new[] { "false", "no", "0" }.Contains(workingValue))
                return FormatBoolean(false);
            else
                return value;
        }
    }
}
