using NBi.Core.Calculation;
using NBi.Core.Calculation.Asserting;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Conversion;

class TextToDateTimeConverter : BaseConverter<string, DateTime?>
{
    public TextToDateTimeConverter(CultureInfo cultureInfo, DateTime? dateTime)
        : base(cultureInfo, dateTime)
    { }

    protected override DateTime? OnExecute(string x, CultureInfo cultureInfo)
        => DateTime.ParseExact(x, cultureInfo.DateTimeFormat.ShortDatePattern + " " + cultureInfo.DateTimeFormat.LongTimePattern, cultureInfo, DateTimeStyles.None);

    protected override PredicateArgs GetPredicateArgs(CultureInfo cultureInfo) => new TextToDateTimePredicateArgs(cultureInfo.Name);

    private class TextToDateTimePredicateArgs : CultureSensitivePredicateArgs
    {
        public TextToDateTimePredicateArgs(string culture)
            : base(culture)
        {
            ColumnType = ColumnType.Text;
            ComparerType = ComparerType.MatchesDateTime;
        }
    }
}
