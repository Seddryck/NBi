using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    class NotImplementedTransformationException : NotImplementedException
    {
        public NotImplementedTransformationException(string className)
            : base($"The native transformation named '{className}' is not implemented in this version of NBi")
        { }
    }
}
