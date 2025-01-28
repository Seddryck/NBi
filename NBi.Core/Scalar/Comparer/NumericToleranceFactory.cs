using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Comparer;

public class NumericToleranceFactory
{
    public NumericTolerance Instantiate(string value)
    {
        var side = SideTolerance.Both;
        
        //Empty string equals zero
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            value = "0";

        value = value.Trim();
        //Check if it's one-sided or not
        if (value.StartsWith("+"))
            side = SideTolerance.More;
        else if (value.Trim().StartsWith("-"))
            side = SideTolerance.Less;

        if (value.Trim().StartsWith("-") || value.Trim().StartsWith("+"))
            value = value[1..];

        //Convert the value to an absolute decimal value
        decimal toleranceDecimal = 0;
        var isDecimal = false;
        isDecimal = decimal.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out toleranceDecimal);
        if (isDecimal)
            return new NumericAbsoluteTolerance(toleranceDecimal, side);

        //Convert the value to an % decimal value
        decimal tolerancePercentage = 0;
        var isPercentage = false;
        if (!isDecimal && !string.IsNullOrEmpty(value) && value.Replace(" ", "").Reverse().ElementAt(0) == '%')
        {
            var percentage = string.Concat(value.Replace(" ", "").Reverse().Skip(1).Reverse());
            isPercentage = decimal.TryParse(percentage, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out tolerancePercentage);
        }
        if (isPercentage)
            return new NumericPercentageTolerance(tolerancePercentage / 100, side);

        //Convert the value to a bounded %
        decimal toleranceBound = 0;
        decimal min = 0;
        decimal max = 0;
        var isBoundedPercentage = false;
        if (!isDecimal && !isPercentage && !string.IsNullOrEmpty(value) && value.Contains('%'))
        {
            var percentage = value.Replace(" ", "")[..value.Replace(" ", "").IndexOf('%')];
            isBoundedPercentage = decimal.TryParse(percentage, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out tolerancePercentage);
            var bound = value.Replace(" ", "")[(value.Replace(" ", "").IndexOf('%') + 1)..].Replace("(", "").Replace(")", "").Replace(":", "").Replace("=", "");

            if (bound.Length > 3 && (bound[..3] == "min" || bound[..3] == "max"))
            {
                isBoundedPercentage = decimal.TryParse(bound[3..], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out toleranceBound);
                if (bound.ToLower().Contains("min"))
                    min = toleranceBound;
                if (bound.ToLower().Contains("max"))
                    max = toleranceBound;
                isBoundedPercentage = (min != max);
            }

        }
        if (isBoundedPercentage)
            return new NumericBoundedPercentageTolerance(tolerancePercentage / 100, min, max);

        throw new ArgumentException(string.Format("Can't convert '{0}' to a double, a percentage or a bounded percentage", value), nameof(value));
    }
}
