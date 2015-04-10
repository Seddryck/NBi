using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
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

        public NumericPercentageTolerance(decimal value)
            : base(value)
        {
            Value = value;
            valueString = (100 * value).ToString(NumberFormatInfo.InvariantInfo);
        }
    }
}
