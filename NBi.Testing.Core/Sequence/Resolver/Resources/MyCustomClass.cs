using NBi.Extensibility.Resolving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Sequence.Resolver.Resources;

public class MyCustomClass : ISequenceResolver
{
    public IList Execute() => new string[] { "myFirstValue", "mySecondValue", "myThirdValue" }.ToList();

    object IResolver.Execute() => Execute();
}
