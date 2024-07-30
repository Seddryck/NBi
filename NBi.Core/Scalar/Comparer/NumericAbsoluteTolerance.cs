using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Comparer
{
    public class NumericAbsoluteTolerance : NumericTolerance
    {
        public override string ValueString
        {
            get
            {
                return Side switch
                {
                    SideTolerance.More => string.Format("+{0}", base.ValueString),
                    SideTolerance.Less => string.Format("-{0}", base.ValueString),
                    _ => base.ValueString,
                };
            }
        }


        public NumericAbsoluteTolerance(decimal value, SideTolerance side)
            : base(value, side)
        {
            Value = value;
        }

        public static NumericAbsoluteTolerance None
            => none;

        private readonly static NumericAbsoluteTolerance none = new (0, SideTolerance.Both);
    }
}
