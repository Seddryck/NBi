using NBi.Core.Sequence.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class SingleVariableInstanceArgs : IInstanceArgs
    {
        public string Name { get; set; }
        public ISequenceResolver<object> Resolver { get; set; }
    }
}
