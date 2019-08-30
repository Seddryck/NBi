using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class NumericToCeiling : AbstractNumericToTruncation
    {
        public NumericToCeiling()
        { }

        protected override decimal Truncate(decimal numeric) => Math.Ceiling(numeric);
    }
}
