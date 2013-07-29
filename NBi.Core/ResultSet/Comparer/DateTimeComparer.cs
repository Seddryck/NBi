using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class DateTimeComparer : BaseComparer
    {

        protected override ComparerResult CompareObjects(object x, object y)
        {
            DateTime rxDateTime, ryDateTime;
            if (x is string)
                rxDateTime = StringParse((string)x);
            else
                rxDateTime = Convert.ToDateTime(x, DateTimeFormatInfo.InvariantInfo);

            if (y is string)
                ryDateTime = StringParse((string)y);
            else
                ryDateTime = Convert.ToDateTime(y, DateTimeFormatInfo.InvariantInfo);

            //Compare DateTimes (with tolerance)
            if (IsEqual(rxDateTime, ryDateTime))
                return ComparerResult.Equality;

            return new ComparerResult(rxDateTime.ToString(NumberFormatInfo.InvariantInfo));
        }

        protected override ComparerResult CompareObjects(object x, object y, object tolerance)
        {
            throw new NotImplementedException("You cannot compare with a DateTime comparer and a tolerance (for the moment).");
        }

        protected bool IsEqual(DateTime x, DateTime y)
        {
            //quick check
            return (x == y);
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
