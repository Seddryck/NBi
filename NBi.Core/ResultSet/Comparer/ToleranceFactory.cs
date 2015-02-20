using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    public class ToleranceFactory
    {

        public static Tolerance Build(IColumnDefinition columnDefinition)
        {
            if (columnDefinition.Role != ColumnRole.Value)
                throw new ArgumentException("The ColumnDefinition must have have a role defined as 'Value' and is defined as 'Key'", "columnDefinition");

            return Build(columnDefinition.Type, columnDefinition.Tolerance);
        }

        public static Tolerance Build(ColumnType type, string value)
        {
            Tolerance tolerance=null;
            switch (type)
            {
                case ColumnType.Text:
                    break;
                case ColumnType.Numeric:
                    tolerance = BuildNumeric(value);
                    break;
                case ColumnType.DateTime:
                    tolerance = new DateTimeTolerance(TimeSpan.Parse(value));
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
                    break;
                case ColumnType.Numeric:
                    tolerance = BuildNumeric("0");
                    break;
                case ColumnType.DateTime:
                    tolerance = new DateTimeTolerance(TimeSpan.Parse("0"));
                    break;
                case ColumnType.Boolean:
                    break;
                default:
                    break;
            }

            return tolerance;
        }

        public static NumericTolerance BuildNumeric(string value)
        {
            //Empty string equals zero
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                value = "0";
            
            //Convert the value to an absolute decimal value
            decimal toleranceDecimal = 0;
            var isDecimal = false;
            isDecimal = decimal.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out toleranceDecimal);
            if (isDecimal)
                return new NumericAbsoluteTolerance(toleranceDecimal);

            //Convert the value to an % decimal value
            decimal tolerancePercentage = 0;
            var isPercentage = false;
            if (!isDecimal && !string.IsNullOrEmpty(value) && value.Replace(" ", "").Reverse().ElementAt(0) == '%')
            {
                var percentage = string.Concat(value.Replace(" ", "").Reverse().Skip(1).Reverse());
                isPercentage = decimal.TryParse(percentage, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out tolerancePercentage);
            }
            if (isPercentage)
                return new NumericPercentageTolerance(tolerancePercentage/100);

            throw new ArgumentException(string.Format("Can't convert '{0}' to a double or a percentage", value), "value");
        }
    }
}
