using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class AnyToAny : INativeTransformation
    {
        public object Evaluate(object value) => "(any)";
    }
}
