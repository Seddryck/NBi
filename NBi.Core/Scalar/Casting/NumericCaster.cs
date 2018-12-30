using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Caster
{
    class NumericCaster : BaseNumericCaster, ICaster<decimal>
    {
        public decimal Execute(object value)
        {
            if (value is decimal)
                return (decimal)value;

            try
            {
                return System.Convert.ToDecimal(value, NumberFormatInfo.InvariantInfo);
            }
            catch
            {
                throw new NBiException($"Can't cast the value '{value}' to a decimal.");
            }
            
        }

        object ICaster.Execute(object value) => Execute(value);
    }
}
