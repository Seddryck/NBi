using NBi.Core;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Sequence.Resolver.Resources
{
    public class MyCustomClass : ISequenceResolver
    {
        public IList Execute() => new string[] { "myFirstValue", "mySecondValue", "myThirdValue" }.ToList();

        object IResolver.Execute() => Execute();
    }
}
