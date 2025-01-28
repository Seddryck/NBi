using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable;

class InternalVariable : RuntimeVariable
{
    public InternalVariable(IScalarResolver resolver)
        : base(resolver) { }
}
