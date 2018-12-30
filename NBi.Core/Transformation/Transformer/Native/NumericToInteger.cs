using NBi.Core.Scalar.Caster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class NumericToInteger : AbstractNumericToTruncation
    {
        public NumericToInteger()
        { }

        protected override decimal Truncate(decimal numeric) => Math.Round(numeric, 0);
    }
}
