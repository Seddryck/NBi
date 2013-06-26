using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    public class BaseComparer
    {
        public bool IsValidNumeric(object value)
        {
            if (value is string && ((string)value) == "(value)")
                return true;

            decimal num = 0;
            var result = Decimal.TryParse(value.ToString()
                                , NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowDecimalPoint
                                , CultureInfo.InvariantCulture
                                , out num);
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

        public bool IsValidDateTime(string value)
        {
            if (value == "(value)")
                return true;

            DateTime dateTime = DateTime.MinValue;
            var result = DateTime.TryParse(value
                                , CultureInfo.InvariantCulture.DateTimeFormat
                                , DateTimeStyles.AllowWhiteSpaces
                                , out dateTime);
            if (!result)
            {
                result = DateTime.TryParse(value
                                , new CultureInfo("fr-fr").DateTimeFormat
                                , DateTimeStyles.AllowWhiteSpaces
                                , out dateTime);
            }
            return result;
        }
    }
}
