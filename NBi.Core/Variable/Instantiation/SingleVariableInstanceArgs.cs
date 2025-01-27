using NBi.Core.Sequence.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable.Instantiation;

public class SingleVariableInstanceArgs : IInstanceArgs
{
    public string Name { get; set; }
    public ISequenceResolver Resolver { get; set; }

    public SingleVariableInstanceArgs(string name, ISequenceResolver resolver)
        => (Name, Resolver) = (name, resolver);

    public IEnumerable<string> Categories { get; set; } = [];
    public IDictionary<string, string> Traits { get; set; } = new Dictionary<string, string>();
}
