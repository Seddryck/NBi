using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable;

public class InstanceVariable : ILoadtimeVariable
{
    private object Value { get; }

    public InstanceVariable(object value)
        => Value = value;

    public object GetValue() => Value;

    public bool IsEvaluated() => true;
}
