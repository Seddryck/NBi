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

            //Convert the value to a bounded %
            decimal toleranceBound = 0;
            decimal min = 0;
            decimal max = 0;
            var isBoundedPercentage = false;
            if (!isDecimal && !isPercentage && !string.IsNullOrEmpty(value) && value.Contains('%'))
            {
                var percentage = value.Replace(" ", "").Substring(0, value.Replace(" ", "").IndexOf('%'));
                isBoundedPercentage = decimal.TryParse(percentage, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out tolerancePercentage);
                var bound = value.Replace(" ", "").Substring(value.Replace(" ", "").IndexOf('%') + 1).Replace("(", "").Replace(")", "").Replace(":", "").Replace("=", "");

                if (bound.Length>3 && (bound.Substring(0, 3) == "min" || bound.Substring(0, 3) == "max"))
                {
                    isBoundedPercentage = decimal.TryParse(bound.Substring(3), NumberStyles.Float, NumberFormatInfo.InvariantInfo, out toleranceBound);
                    if (bound.ToLower().Contains("min"))
                        min = toleranceBound;
                    if (bound.ToLower().Contains("max"))
                        max = toleranceBound;
                    isBoundedPercentage = (min != max);
                }
                
            }
            if (isBoundedPercentage)
                return new NumericBoundedPercentageTolerance(tolerancePercentage/100, min, max);

            throw new ArgumentException(string.Format("Can't convert '{0}' to a double, a percentage or a bounded percentage", value), "value");
        }
    }
}
