using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable.Instantiation
{
    public class Instance
    {
        public IDictionary<string, IVariable> Variables { get; }
        public IEnumerable<string> Categories { get; }
        public IDictionary<string, string> Traits { get; }

        public Instance(IDictionary<string, IVariable> variables, IEnumerable<string> categories, IDictionary<string, string> traits)
        {
            Variables = variables;
            Categories = categories ?? new List<string>();
            Traits = traits ?? new Dictionary<string, string>();
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
                : base(new Dictionary<string, IVariable>(),null, null)
            { }

            public override string GetName() => string.Empty;
        }
    }
}
