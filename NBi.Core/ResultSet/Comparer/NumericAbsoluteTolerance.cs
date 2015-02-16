using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
    public class NumericAbsoluteTolerance : NumericTolerance
    {
        public NumericAbsoluteTolerance(decimal value)
            : base(value)
        {
            Value = value;
        }

        public static NumericAbsoluteTolerance None
        {
            get
            {
                return new NumericAbsoluteTolerance(0);
            }
        }
    }
}
