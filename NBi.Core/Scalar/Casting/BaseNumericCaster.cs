using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Caster
{
    class BaseNumericCaster
    {
        public virtual bool IsValid(object value)
        {
            if (value is sbyte
                    || value is byte
                    || value is ushort
                    || value is short
                    || value is uint
                    || value is int
                    || value is ulong
                    || value is long
                    || value is float
                    || value is double
                    || value is decimal)
                return true;
            
            if (value is string && ((string)value) == "(value)")
                return true;

            if (value is string && ((string)value) == "(any)")
                return true;

            return IsParsableNumeric(value);
        }

        protected bool IsParsableNumeric(object value)
        {
            var result = Decimal.TryParse(value.ToString()
                                , NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowDecimalPoint
                                , CultureInfo.InvariantCulture
                                , out decimal num);
            //The first method is not enough, you can have cases where this method returns false but the value is effectively a numeric. The problem is in the .ToString() on the object where you apply the regional settings for the numeric values.
            //The second method gives a better result but unfortunately generates an exception.
            if (!result)
            {
                try
                {
                    num = Convert.ToDecimal(value, NumberFormatInfo.InvariantInfo);
                    result = true;
                }
                catch (Exception)
                {

                    result = false;
                }
            }
            return result;
        }
    }
}
