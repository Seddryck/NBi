using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Variable
{
    public class RevokeVariableAction : IVariableAction
    {
        public IReadOnlyCollection<string> Names { get; private set; }

        public RevokeVariableAction(IEnumerable<string> names)
        {
            Names = (IReadOnlyCollection<string>)names;
        }

        public void Execute(GenerationState state)
        {
            foreach (var name in Names)
            {
                if (state.Variables.ContainsKey(name))
                    state.Variables.Remove(name);
            }
        }

        public string Display
        {
            get
            {
                if (Names.Count>1)
                    return string.Format($"Revoking variables '{string.Join("', '", Names)}'.");
                else
                    return string.Format($"Revoking variable '{Names.ElementAt(0)}'.");
            }
        }
    }
}
