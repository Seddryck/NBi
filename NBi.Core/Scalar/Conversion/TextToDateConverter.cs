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
    class TextToDateConverter : BaseConverter<string, DateTime>
    {
        public TextToDateConverter(CultureInfo cultureInfo, DateTime? dateTime)
            : base(cultureInfo, dateTime)
        { }

        protected override DateTime OnExecute(string x, CultureInfo cultureInfo) 
            => DateTime.ParseExact(x, cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo, DateTimeStyles.None);

        protected override IPredicateInfo GetPredicateInfo(CultureInfo cultureInfo) => new PredicateInfo(cultureInfo.Name);

        private class PredicateInfo : IPredicateInfo, ICultureSensitivePredicateInfo
        {
            public PredicateInfo(string culture)
            {
                Culture = culture;
            }

            public ColumnType ColumnType { get => ColumnType.Text; set => throw new NotImplementedException(); }
            public ComparerType ComparerType => ComparerType.MatchesDate;
            public IColumnIdentifier Operand { get => null; set => throw new NotImplementedException(); }
            public bool Not { get => false; set => throw new NotImplementedException(); }
            public string Culture { get; }
        }
    }
}
