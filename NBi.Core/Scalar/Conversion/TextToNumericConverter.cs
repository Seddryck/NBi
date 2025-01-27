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

class TextToNumericConverter : BaseConverter<string, decimal>
{
    public TextToNumericConverter(CultureInfo cultureInfo, decimal? defaultValue)
        : base(cultureInfo, defaultValue)
    { }

    protected override decimal OnExecute(string x, CultureInfo cultureInfo)
        => decimal.Parse(x, NumberStyles.Number & ~NumberStyles.AllowThousands, cultureInfo.NumberFormat);

    protected override PredicateArgs GetPredicateArgs(CultureInfo cultureInfo) => new TextToNumericPredicateArgs(cultureInfo.Name);

    private class TextToNumericPredicateArgs : CultureSensitivePredicateArgs
    {
        public TextToNumericPredicateArgs(string culture)
            : base(culture)
        {
            ColumnType = ColumnType.Text;
            ComparerType = ComparerType.MatchesNumeric;
        }
    }
}
