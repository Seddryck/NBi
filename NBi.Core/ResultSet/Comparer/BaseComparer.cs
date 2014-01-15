using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    abstract class BaseComparer
    {
        public ComparerResult Compare(object x, object y)
        {
            var eq = CompareBasic(x,y);
            if (eq != null)
                return eq;

            return CompareObjects(x,y);
        }

        public ComparerResult Compare(object x, object y, Rounding rounding)
        {
            var eq = CompareBasic(x, y);
            if (eq != null)
                return eq;

            return CompareObjects(x, y, rounding);
        }

        public ComparerResult Compare(object x, object y, Tolerance tolerance)
        {
            var eq = CompareBasic(x, y);
            if (eq != null)
                return eq;

            return CompareObjects(x, y, tolerance);
        }

        protected abstract bool IsValidObject (object x);
        protected abstract ComparerResult CompareObjects(object x, object y);
        protected abstract ComparerResult CompareObjects(object x, object y, Tolerance tolerance);
        protected abstract ComparerResult CompareObjects(object x, object y, Rounding rounding);

        protected ComparerResult CompareBasic(object x, object y)
        {
            if (x is string && ((string)x) == "(null)")
                x = null;

            if (y is string && ((string)y) == "(null)")
                y = null;

            if (EqualByGeneric(x, y))
                return ComparerResult.Equality;

            var eq = EqualByNull(x, y);
            if (eq != null)
                return eq;

            return null;
        }


        private ComparerResult EqualByNull(object x, object y)
        {
            if (x == null && y == null)
                return ComparerResult.Equality;

            if (x == null || y == null)
                return new ComparerResult("(null)");

            return null;
        }

        protected bool EqualByGeneric(object x, object y)
        {
            if (x is string && ((string)x) == "(value)")
                return y != null && IsValidObject(y);

            if (y is string && ((string)y) == "(value)")
                return x != null && IsValidObject(x);

            if (x is string && ((string)x) == "(any)")
                return y == null || IsValidObject(y);

            if (y is string && ((string)y) == "(any)")
                return x == null || IsValidObject(x);

            return false;
        }

        
        public static bool IsValidNumeric(object value)
        {
            if (value is string && ((string)value) == "(value)")
                return true;

            if (value is string && ((string)value) == "(any)")
                return true;

            return IsParsableNumeric(value);
        }

        protected static bool IsParsableNumeric(object value)
        {
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

        public static bool IsValidDateTime(string value)
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

        internal static bool IsValidInterval(object value)
        {
            if (!(value is string))
                return false;

            var valueString = ((string)value).Replace(" ","");

            if (valueString.StartsWith("(") && valueString.EndsWith(")"))
                return true;

            if (valueString.StartsWith("[") || valueString.StartsWith("]")
                && valueString.EndsWith("[") || valueString.EndsWith("]")
                && valueString.Contains(";"))
                return true;

            return false;
        }
    }
}
