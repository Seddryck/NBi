using NBi.Core;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Acceptance.Resources
{
    public class CustomSequenceMonths : ISequenceResolver
    {
        public CustomSequenceMonths()
        { }

        object IResolver.Execute() => Execute();

        public IList Execute() => new DateTime[] 
        { 
            new DateTime(2016, 1, 1), 
            new DateTime(2016, 2, 1), 
            new DateTime(2016, 3, 1) 
        };
    }
}
