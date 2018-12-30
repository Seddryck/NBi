using NBi.Core.Scalar.Caster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class NumericToFloor : AbstractNumericToTruncation
    {
        public NumericToFloor()
        { }

        protected override decimal Truncate(decimal numeric) => Math.Floor(numeric);
    }
}
