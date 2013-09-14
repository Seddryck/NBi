using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class DateTimeComparer : BaseComparer
    {

        protected override ComparerResult CompareObjects(object x, object y)
        {
            var rxDateTime = ConvertToDate(x);
            var ryDateTime = ConvertToDate(y);

            //Compare DateTimes (without tolerance)
            if (IsEqual(rxDateTime, ryDateTime))
                return ComparerResult.Equality;

            return new ComparerResult(rxDateTime.ToString(DateTimeFormatInfo.InvariantInfo));
        }

        public ComparerResult Compare(object x, object y, TimeSpan tolerance)
        {
            return base.Compare(x, y, tolerance);
        }

        public ComparerResult Compare(object x, object y, string tolerance)
        {
            return base.Compare(x, y, tolerance);
        }

        public ComparerResult Compare(object x, object y, DateTimeRounding rounding)
        {
            var rxDateTime = ConvertToDate(x);
            var ryDateTime = ConvertToDate(y);

            rxDateTime = rounding.GetValue(rxDateTime);
            ryDateTime = rounding.GetValue(ryDateTime);

            return CompareObjects(x, y);
        }

        protected override ComparerResult CompareObjects(object x, object y, object tolerance)
        {
            var rxDateTime = ConvertToDate(x);
            var ryDateTime = ConvertToDate(y);
            
            if (!(tolerance is string))
                throw new ArgumentException(string.Format("Tolerance for a dateTime comparer must be a string and is a '{0}'.", tolerance.GetType().Name), "tolerance");

            //Convert the value to a timespan value
            TimeSpan toleranceTimeSpan;
            var isTimeSpan = false;
            if ((tolerance is TimeSpan))
            {
                toleranceTimeSpan = (TimeSpan)tolerance;
                isTimeSpan = true;
            }
            else
                isTimeSpan = TimeSpan.TryParse((string)tolerance, DateTimeFormatInfo.InvariantInfo, out toleranceTimeSpan);

            if (!isTimeSpan)
                throw new ArgumentException(string.Format("Tolerance for a dateTime comparer must be a TimeSpan but '{0}' is not recognized as a valid TimeSpan value.", tolerance), "tolerance");

            //Compare dateTimes (with tolerance)
            if (IsEqual(rxDateTime, ryDateTime, toleranceTimeSpan))
                return ComparerResult.Equality;

            return new ComparerResult(rxDateTime.ToString(DateTimeFormatInfo.InvariantInfo));
        }

        protected DateTime ConvertToDate(object x)
        {
            if (x is string)
                return StringParse((string)x);
            
            return Convert.ToDateTime(x, DateTimeFormatInfo.InvariantInfo);
        }

        protected bool IsEqual(DateTime x, DateTime y)
        {
            //quick check
            return (x == y);
        }

        protected bool IsEqual(DateTime x, DateTime y, TimeSpan tolerance)
        {
            //Console.WriteLine("IsEqual: {0} {1} {2} {3} {4} {5}", x, y, tolerance, Math.Abs(x - y), x == y, Math.Abs(x - y) <= tolerance);

            //quick check
            if (x == y)
                return true;

            //Stop checks if tolerance is set to 0
            if (tolerance.Ticks==0)
                return false;

            //include some math[Time consumming] (Tolerance needed to validate)
            return (x.Subtract(y).Duration() <= tolerance);
        }

        protected DateTime StringParse(string value)
        {
            bool result = false;
            DateTime dateTime;
            result = ValidDateTime(value, out dateTime);
            if (!result)
                throw new ArgumentException(string.Format("'{0}' is not recognized as a valid date", value), "value");
            
            return dateTime;
        }
  
        private bool ValidDateTime(string value, out DateTime dateTime)
        {
            dateTime = DateTime.MinValue;
            var result = DateTime.TryParse(value,
                CultureInfo.InvariantCulture.DateTimeFormat,
                DateTimeStyles.AllowWhiteSpaces,
                out dateTime);
            if (!result)
            {
                result = DateTime.TryParse(value,
                    new CultureInfo("fr-fr").DateTimeFormat,
                    DateTimeStyles.AllowWhiteSpaces,
                    out dateTime);
            }

            return result;
        }

        protected override bool IsValidObject(object x)
        {
            return (x is DateTime || (x is string && IsValidDateTime((string)x)));
        }
    }
}
