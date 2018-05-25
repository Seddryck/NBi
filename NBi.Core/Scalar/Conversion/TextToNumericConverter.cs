using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate.Text;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Conversion
{
    class TextToNumericConverter : BaseConverter<string, decimal>
    {
        public TextToNumericConverter(CultureInfo cultureInfo, decimal? defaultValue)
            : base(cultureInfo, defaultValue)
        { }

        protected override decimal OnExecute(string x, CultureInfo cultureInfo)
            => Decimal.Parse(x, NumberStyles.Number & ~NumberStyles.AllowThousands, cultureInfo.NumberFormat);

        protected override IPredicateInfo GetPredicateInfo(CultureInfo cultureInfo) => new PredicateInfo(cultureInfo.Name);

        private class PredicateInfo : IPredicateInfo, ICultureSensitivePredicateInfo
        {
            public PredicateInfo(string culture)
            {
                Culture = culture;
            }

            public ColumnType ColumnType { get => ColumnType.Text; set => throw new NotImplementedException(); }
            public ComparerType ComparerType => ComparerType.MatchesNumeric;
            public IColumnIdentifier Operand { get => null; set => throw new NotImplementedException(); }
            public bool Not { get => false; set => throw new NotImplementedException(); }
            public string Culture { get; }
        }
    }
}
