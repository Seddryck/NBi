using NBi.Core.ResultSet;
using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Comparer
{
    public class RoundingFactory
    {

        public static Rounding Build(IColumnDefinition columnDefinition)
        {
            if (columnDefinition.Role != ColumnRole.Value)
                throw new ArgumentException("The ColumnDefinition must have have a role defined as 'Value' and is defined as 'Key'", nameof(columnDefinition));

            Rounding rounding=null;
            switch (columnDefinition.Type)
            {
                case ColumnType.Text:
                    break;
                case ColumnType.Numeric:
                    rounding = new NumericRounding(decimal.Parse(columnDefinition.RoundingStep, NumberFormatInfo.InvariantInfo), columnDefinition.RoundingStyle);
                    break;
                case ColumnType.DateTime:
                    rounding = new DateTimeRounding(TimeSpan.Parse(columnDefinition.RoundingStep, NumberFormatInfo.InvariantInfo), columnDefinition.RoundingStyle);
                    break;
                case ColumnType.Boolean:
                    break;
                default:
                    break;
            }

            return rounding;
        }
    }
}
