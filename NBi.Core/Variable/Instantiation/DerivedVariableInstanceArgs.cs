using NBi.Core.Sequence.Resolver;
using NBi.Core.Transformation;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable.Instantiation;

public class DerivedVariableInstanceArgs(string name, ISequenceResolver resolver, IDictionary<string, DerivationArgs> derivations) : SingleVariableInstanceArgs(name, resolver)
{
    public IDictionary<string, DerivationArgs> Derivations { get; set; } = derivations;
}

public class DerivationArgs(string source, ITransformer transformer)
{
    public string Source { get; set; } = source;
    public ITransformer Transformer { get; set; } = transformer;
}
