using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Predefined
{
    internal class DaysOfWeekBuilder : BaseBuilder
    {
        protected override void InternalBuild()
        {
            Result =  Build(Culture, DayOfWeek.Monday, ToTitleCase);
        }

        public IEnumerable<string> Build(CultureInfo culture, DayOfWeek firstDay, Func<string, string> caseModifier)
        {
            var list = new List<string>(7);
            for (int i = (int)firstDay; i < list.Capacity; i++)
                list.Add(culture.DateTimeFormat.GetDayName((DayOfWeek)i));

            for (int i = 0; i < list.Capacity - list.Count; i++)
                list.Add(culture.DateTimeFormat.GetDayName((DayOfWeek)i));

            if (caseModifier != null)
                for (int i = 0; i < list.Capacity; i++)
                    list[i] = caseModifier(list[i]);

            return list;
        }
    }
}
