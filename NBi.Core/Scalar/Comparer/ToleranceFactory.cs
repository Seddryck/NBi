using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Scalar.Comparer
{
    public class ToleranceFactory
    {

        public static Tolerance Instantiate(IColumnDefinition columnDefinition)
        {
            if (columnDefinition.Role != ColumnRole.Value)
                throw new ArgumentException("The ColumnDefinition must have have a role defined as 'Value' and is defined as 'Key'", "columnDefinition");

            return Instantiate(columnDefinition.Type, columnDefinition.Tolerance);
        }

        public static Tolerance Instantiate(ColumnType type, string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return null;

            Tolerance tolerance=null;
            switch (type)
            {
                case ColumnType.Text:
                    tolerance = new TextToleranceFactory().Instantiate(value);
                    break;
                case ColumnType.Numeric:
                    tolerance = new NumericToleranceFactory().Instantiate(value);
                    break;
                case ColumnType.DateTime:
                    tolerance = new DateTimeToleranceFactory().Instantiate(value);
                    break;
                case ColumnType.Boolean:
                    break;
                default:
                    break;
            }

            return tolerance;
        }

        public static Tolerance None(ColumnType type)
        {
            Tolerance tolerance = null;
            switch (type)
            {
                case ColumnType.Text:
                    tolerance = TextSingleMethodTolerance.None;
                    break;
                case ColumnType.Numeric:
                    tolerance = NumericAbsoluteTolerance.None;
                    break;
                case ColumnType.DateTime:
                    tolerance = DateTimeTolerance.None;
                    break;
                case ColumnType.Boolean:
                    break;
                default:
                    break;
            }

            return tolerance;
        }

        
    }
}
