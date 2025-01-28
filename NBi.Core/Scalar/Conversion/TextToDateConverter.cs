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

class TextToDateConverter : BaseConverter<string, DateTime?>
{
    public TextToDateConverter(CultureInfo cultureInfo, DateTime? dateTime)
        : base(cultureInfo, dateTime)
    { }

    protected override DateTime? OnExecute(string x, CultureInfo cultureInfo) 
        => DateTime.ParseExact(x, cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo, DateTimeStyles.None);

    protected override PredicateArgs GetPredicateArgs(CultureInfo cultureInfo) => new TextToDatePredicateArgs(cultureInfo.Name);

    private class TextToDatePredicateArgs : CultureSensitivePredicateArgs
    {
        public TextToDatePredicateArgs(string culture)
            : base(culture)
        {
            ColumnType = ColumnType.Text;
            ComparerType = ComparerType.MatchesDate;
        }
    }
}
