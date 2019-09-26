using NBi.Core.Sequence.Resolver;
using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable.Instantiation
{
    public class DerivedVariableInstanceArgs : SingleVariableInstanceArgs
    {
        public IDictionary<string, DerivationArgs> Derivations { get; set; }
    }

    public class DerivationArgs
    {
        public string Source { get; set; }
        public ITransformer Transformer { get; set; }
    }
}
