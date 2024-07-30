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
            return value switch
            {
                string x => PresentString(x),
                decimal x => PresentNumeric(x),
                short x => PresentNumericObject(x),
                int x => PresentNumericObject(x),
                double x => PresentNumericObject(x),
                float x => PresentNumericObject(x),
                _ => PresentString(value.ToString() ?? string.Empty),
            };
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
