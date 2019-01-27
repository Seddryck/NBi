using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Casting
{
    class ImplicitCaster : ICaster<object>
    {
        public object Execute(object value) => value;

        object ICaster.Execute(object value) => Execute(value);

        public bool IsValid(object value) => true;
    }
}
