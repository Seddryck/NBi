using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Casting
{
    class UntypedCaster : ICaster<object>
    {
        public object Execute(object? value)
            => value ?? new NBiException($"Can't cast the value '(null)'");

        object ICaster.Execute(object? value) => Execute(value);

        public bool IsValid(object? value) => true;
        public bool IsStrictlyValid(object? obj) => true;
    }
}
