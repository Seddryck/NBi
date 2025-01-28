using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native;

public interface INativeTransformation
{
    object? Evaluate(object? value);
}
