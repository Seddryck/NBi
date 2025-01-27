using System;
using System.Linq;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case;

public class ScopeCaseAction : IMultiCaseAction
{
    public string Name { get; set; }
    
    public ScopeCaseAction(string name)
    {
        Name = name;
    }
    public void Execute(GenerationState state)
    {
        if (!state.CaseCollection.ContainsKey(Name))
            state.CaseCollection.Add(Name, new CaseSet());
        state.CaseCollection.CurrentScopeName = Name;
    }

    public virtual string Display => $"Focussing on test cases set named '{Name}'";
}
