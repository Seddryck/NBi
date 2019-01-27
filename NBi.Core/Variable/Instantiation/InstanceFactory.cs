using NBi.Core.Sequence.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable.Instantiation
{
    public class InstanceFactory
    {
        public IEnumerable<Instance> Instantiate(IInstanceArgs args)
        {
            switch (args)
            {
                case DefaultInstanceArgs d: return new[] { Instance.Default };
                case SingleVariableInstanceArgs s: return Instantiate(s.Name, s.Resolver);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerable<Instance> Instantiate(string variableName, ISequenceResolver resolver)
        {
            foreach (var obj in resolver.Execute())
            {
                var instanceVariable = new InstanceVariable(obj);
                yield return new Instance(new Dictionary<string, ITestVariable>() { { variableName, instanceVariable } });
            }
        }
    }
}
