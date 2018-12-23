using NBi.Core.Sequence.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class InstanceFactory
    {
        public IEnumerable<Instance> Instantiate(string variableName, ISequenceResolver<object> resolver)
        {
            foreach (var obj in resolver.Execute())
            {
                var instanceVariable = new InstanceVariable(obj);
                yield return new Instance(new Dictionary<string, InstanceVariable>() { { variableName, instanceVariable } });
            }
        }
    }
}
