using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Casting;

class BaseNumericCaster
{
    public virtual bool IsStrictlyValid(object? value)
        => value switch
        {
            null => false,
            sbyte => true,
            byte => true,
            ushort => true,
            short => true,
            uint => true,
            int => true,
            ulong => true,
            long => true,
            float => true,
            double => true,
            decimal => true,
            string v => IsParsableNumeric(v),
            _ => false
        };
        
    public virtual bool IsValid(object? value)
        => value switch
        {
            null => false,
            string v when v == "(value)" || v == "(any)" => true,
            _ => IsStrictlyValid(value)
        };

    protected virtual bool IsParsableNumeric(object value)
    {
        if (value == null)
            return false;

        var result = decimal.TryParse(value.ToString()
                            , NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowDecimalPoint
                            , CultureInfo.InvariantCulture
                            , out _);
        ////The first method is not enough, you can have cases where this method returns false but the value is effectively a numeric. The problem is in the .ToString() on the object where you apply the regional settings for the numeric values.
        ////The second method gives a better result but unfortunately generates an exception.
        //if (!result)
        //{
        //    try
        //    {
        //        var num = Convert.ToDecimal(value, NumberFormatInfo.InvariantInfo);
        //        result = true;
        //    }
        //    catch (Exception)
        //    {

        //        result = false;
        //    }
        //}
        return result;
    }
}
