using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
    public class NumericPercentageTolerance : NumericTolerance
    {
        public override string ValueString
        {
            get
            {
                return base.ValueString + "%";
            }
        }

        public NumericPercentageTolerance(decimal value)
            : base(value)
        {
            Value = value;
        }
    }
}
