using System;
using System.Globalization;
using System.Linq;
using NBi.Core.Members.Ranges;

namespace NBi.Model.Items.Ranges;

public class DateRange : ValueRange, IDateRange
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string CultureSymbol { get; set; }
    public string Format { get; set; }

    public CultureInfo Culture
    {
        get
        {
            return new CultureInfo(CultureSymbol);
        }
        set
        {
            CultureSymbol = value.Name;
        }
    }
}
