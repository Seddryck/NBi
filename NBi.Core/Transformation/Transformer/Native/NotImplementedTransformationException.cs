using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native;

class NotImplementedTransformationException : NBiException
{
    public NotImplementedTransformationException(string className)
        : base($"The native transformation named '{className}' is not implemented in this version of NBi")
    { }
}

class MissingOrUnexpectedParametersTransformationException : NBiException
{
    public MissingOrUnexpectedParametersTransformationException(string className, int parameterCount)
        : base($"The native transformation named '{className}' is expecting a different count of parameters than {parameterCount}")
    { }
}
