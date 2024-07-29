using NBi.Core.ResultSet;
using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Comparer
{
    public class RoundingFactory
    {

        public static Rounding? Build(IColumnDefinition columnDefinition)
        {
            if (columnDefinition.Role != ColumnRole.Value)
                throw new ArgumentException("The ColumnDefinition must have have a role defined as 'Value' and is defined as 'Key'", nameof(columnDefinition));

            Rounding? rounding = columnDefinition.Type switch
            {
                ColumnType.Numeric => new NumericRounding(decimal.Parse(columnDefinition.RoundingStep, NumberFormatInfo.InvariantInfo), columnDefinition.RoundingStyle),
                ColumnType.DateTime => new DateTimeRounding(TimeSpan.Parse(columnDefinition.RoundingStep, NumberFormatInfo.InvariantInfo), columnDefinition.RoundingStyle),
                _ => null
            };

            return rounding;
        }
    }
}
