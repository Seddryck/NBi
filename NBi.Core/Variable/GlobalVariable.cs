using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable;

public class GlobalVariable : RuntimeVariable
{
    public GlobalVariable(IScalarResolver resolver)
        : base(resolver) { }
}
