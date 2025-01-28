using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Comparer;

public class NumericPercentageTolerance : NumericTolerance
{
    private string valueString;
    public override string ValueString
    {
        get
        {
            return valueString + "%";
        }
    }

    public NumericPercentageTolerance(decimal value, SideTolerance side)
        : base(value, side)
    {
        Value = value;
        valueString = (100 * value).ToString(NumberFormatInfo.InvariantInfo);
        switch (side)
        {
            case SideTolerance.Both:
                break;
            case SideTolerance.More:
                valueString = string.Format("+{0}", valueString);
                break;
            case SideTolerance.Less:
                valueString = string.Format("-{0}", valueString);
                break;
            default:
                break;
        }
    }
}
