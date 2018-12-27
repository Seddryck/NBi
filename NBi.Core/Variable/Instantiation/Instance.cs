using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable.Instantiation
{
    public class Instance
    {
        public IDictionary<string, ITestVariable> Variables { get; }

        public Instance(IDictionary<string, ITestVariable> variables)
        {
            Variables = variables;
        }

        public virtual string GetName() => Variables.ElementAt(0).Value.GetValue().ToString();

        public bool IsDefault
        {
            get => this == Default;
        }
        public static Instance Default { get; } = new DefaultInstance();

        public class DefaultInstance : Instance
        {
            public DefaultInstance()
                : base(new Dictionary<string, ITestVariable>())
            { }

            public override string GetName() => string.Empty;
        }
    }
}
