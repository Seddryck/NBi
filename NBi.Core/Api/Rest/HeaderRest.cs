using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Api.Rest
{
    class HeaderRest
    {
        public IScalarResolver<string> Name { get; }
        public IScalarResolver<string> Value { get; }

        public HeaderRest(IScalarResolver<string> name, IScalarResolver<string> value)
            => (Name, Value) = (name, value);
    }
}
