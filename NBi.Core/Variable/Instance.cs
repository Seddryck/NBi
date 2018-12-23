using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class Instance
    {
        public IDictionary<string, InstanceVariable> Variables { get; }

        public Instance(IDictionary<string, InstanceVariable> variables)
        {
            Variables = variables;
        }

        public string GetName() => Variables.ElementAt(0).Value.GetValue().ToString();
    }
}
