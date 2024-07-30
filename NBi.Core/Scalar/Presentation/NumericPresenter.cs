using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Presentation
{
    public class NumericPresenter : BasePresenter
    {
        protected override string PresentNotNull(object value)
        {
            var typeName = value.GetType().Name.ToLower();
            switch (value)
            {
                case string x   : return PresentString(x);
                case decimal x  : return PresentNumeric(x);
                case short x    : return PresentNumericObject(x);
                case int x      : return PresentNumericObject(x);
                case double x   : return PresentNumericObject(x);
                case float x    : return PresentNumericObject(x);
                default:
                    return PresentString(value.ToString() ?? string.Empty);
            }
        }

        protected string PresentNumeric(decimal value) => value.ToString("G29", CultureInfo.InvariantCulture);

        protected string PresentNumericObject(object value) => PresentNumeric(Convert.ToDecimal(value));

        protected string PresentString(string value)
        {   
            try 
	        {	        
		        var valueDecimal = Convert.ToDecimal(value, NumberFormatInfo.InvariantInfo);
                return PresentNumeric(valueDecimal);
	        }
	        catch (Exception)
	        {
                return value;
	        }
        }
    }
}
