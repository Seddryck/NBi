using NBi.Core.Scalar.Casting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Comparer
{
    abstract class BaseComparer
    {
        public ComparerResult Compare(object? x, object? y)
            => CompareBasic(x,y) ?? CompareObjects(x!,y!);

        public ComparerResult Compare(object? x, object? y, Rounding rounding)
        {
            var eq = CompareBasic(x, y);
            if (eq != null)
                return eq;

            return CompareObjects(x, y, rounding);
        }

        public ComparerResult Compare(object? x, object? y, Tolerance? tolerance)
        {
            var eq = CompareBasic(x, y);
            if (eq != null)
                return eq;

            if (tolerance is null)
                return new ComparerResult(x is null ? "(null)" : x.ToString() ?? "(empty)");

            return CompareObjects(x, y, tolerance);
        }

        protected abstract bool IsValidObject (object x);
        protected abstract ComparerResult CompareObjects(object? x, object? y);
        protected abstract ComparerResult CompareObjects(object? x, object? y, Tolerance tolerance);
        protected abstract ComparerResult CompareObjects(object? x, object? y, Rounding rounding);

        protected ComparerResult? CompareBasic(object? x, object? y)
        {
            if (x is string v && v == "(null)")
                x = null;

            if (y is string v1 && v1 == "(null)")
                y = null;

            if (EqualByRangeValue(x, y))
                return ComparerResult.Equality;

            var eq = EqualByNull(x, y);
            if (eq != null)
                return eq;

            return null;
        }

        protected virtual ComparerResult? EqualByNull(object? x, object? y)
        {
            if (x == null && y == null)
                return ComparerResult.Equality;

            if (y==null && x is string xStr && xStr == "(blank)")
                return ComparerResult.Equality;

            if (x==null && y is string yStr && yStr == "(blank)")
                return ComparerResult.Equality;

            if (x == null || y == null)
                return new ComparerResult("(null)");

            return null;
        }

        protected bool EqualByRangeValue(object? x, object? y)
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
