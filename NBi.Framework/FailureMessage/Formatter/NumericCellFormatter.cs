using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Unit.Framework.FailureMessage.Formatter
{
    public class NumericCellFormatter : CellFormatter
    {
        protected override string FormatNotNull(object value)
        {
            var typeName = value.GetType().Name.ToLower();
            switch (typeName)
            {
                case "string": return FormatString((string)value);
                case "decimal": return FormatNumeric((decimal) value);
                case "short": return FormatNumericObject(value);
                case "int": return FormatNumericObject(value);
                case "double": return FormatNumericObject(value);
                case "float": return FormatNumericObject(value);
                default:
                    return FormatString(value.ToString());
            }
        }

        protected string FormatNumeric(decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }

        protected string FormatNumericObject(object value)
        {
            return FormatNumeric(Convert.ToDecimal(value));
        }

        protected string FormatString(string value)
        {   
            try 
	        {	        
		        var valueDecimal = Convert.ToDecimal(value, NumberFormatInfo.InvariantInfo);
                return FormatNumeric(valueDecimal);
	        }
	        catch (Exception)
	        {
                return value;
	        }
        }
    }
}
