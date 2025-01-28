using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Predefined;

internal class MonthsOfYearBuilder : BaseBuilder
{
    protected override void InternalBuild()
    {
        Result =  Build(Culture, 1, 2013, ToTitleCase);
    }

    public IEnumerable<string> Build(CultureInfo culture, int firstMonth, int year, Func<string, string> caseModifier)
    {
        var monthCount = culture.Calendar.GetMonthsInYear(year);

        var list = new List<string>(monthCount);
        for (int i = firstMonth; i <= list.Capacity; i++)
            list.Add(culture.DateTimeFormat.GetMonthName(i));

        for (int i = 1; i < list.Capacity - list.Count; i++)
            list.Add(culture.DateTimeFormat.GetMonthName(i));

        if (caseModifier != null)
            list.ForEach(d => d = caseModifier(d));

        return list;
    }
}
