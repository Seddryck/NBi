using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Ranges
{
    internal class DateRangeBuilder : BaseBuilder
    {
        protected new IDateRange Range
            => (IDateRange)(base.Range ?? throw new NullReferenceException());

        protected override void InternalBuild()
        {
            if (string.IsNullOrEmpty(Range.Format))
                Result = Build(Range.Start, Range.End, Range.Culture, ToShortDatePattern);
            else
                Result = Build(Range.Start, Range.End, Range.Culture, Range.Format);
        }

        public IEnumerable<string> Build(DateTime start, DateTime end, CultureInfo culture, Func<CultureInfo, DateTime,  string> dateFormatter)
        {
            var list = new List<string>();

            var date= start;
            
            while (date <= end)
            {
                var dateString = dateFormatter(culture, date);
                list.Add(dateString);
                date = date.AddDays(1);
            }

            return list;
        }

        public IEnumerable<string> Build(DateTime start, DateTime end, CultureInfo culture, string format)
        {
            var list = new List<string>();

            var date = start;

            while (date <= end)
            {
                var dateString = date.ToString(format, culture.DateTimeFormat);
                list.Add(dateString);
                date = date.AddDays(1);
            }

            return list;
        }

        protected string ToLongDatePattern(CultureInfo culture, DateTime value)
        {
            return value.ToString(culture.DateTimeFormat.LongDatePattern);
        }

        protected string ToShortDatePattern(CultureInfo culture, DateTime value)
        {
            return value.ToString(culture.DateTimeFormat.ShortDatePattern);
        }
    }
}
