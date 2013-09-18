﻿using System;
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
            return base.Compare(x, y, new DateTimeTolerance(tolerance));
        }

        public ComparerResult Compare(object x, object y, string tolerance)
        {
            return base.Compare(x, y, new DateTimeTolerance(TimeSpan.Parse(tolerance)));
        }

        public ComparerResult CompareObjects(object x, object y, DateTimeRounding rounding)
        {
            var rxDateTime = ConvertToDate(x);
            var ryDateTime = ConvertToDate(y);

            rxDateTime = rounding.GetValue(rxDateTime);
            ryDateTime = rounding.GetValue(ryDateTime);

            return CompareObjects(rxDateTime, ryDateTime);
        }

        protected override ComparerResult CompareObjects(object x, object y, Rounding rounding)
        {
            if (!(rounding is DateTimeRounding))
                throw new ArgumentException("Rounding must be of type 'DateTimeRounding'");

            return CompareObjects(x, y, (DateTimeRounding)rounding);
        }

        protected override ComparerResult CompareObjects(object x, object y, Tolerance tolerance)
        {
            if (!(tolerance is DateTimeTolerance))
                throw new ArgumentException("Tolerance must be of type 'DateTimeTolerance'");

            return CompareObjects(x, y, (DateTimeTolerance)tolerance);
        }

        protected ComparerResult CompareObjects(object x, object y, DateTimeTolerance tolerance)
        {
            var rxDateTime = ConvertToDate(x);
            var ryDateTime = ConvertToDate(y);
            
            //Compare dateTimes (with tolerance)
            if (IsEqual(rxDateTime, ryDateTime, tolerance.TimeSpan))
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
