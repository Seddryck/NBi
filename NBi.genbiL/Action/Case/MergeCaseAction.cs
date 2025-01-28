using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case;

public class MergeCaseAction : IMultiCaseAction
{
    
    public string MergedScope { get; private set; }
    public MergeCaseAction(string mergedScope) => MergedScope = mergedScope;

    public void Execute(GenerationState state)
    {
        if (!state.CaseCollection.ContainsKey(MergedScope))
            throw new ArgumentException($"Scope '{MergedScope}' doesn't exist.");

        var dr = state.CaseCollection[MergedScope].Content.CreateDataReader();
        state.CaseCollection.CurrentScope.Content.Load(dr, LoadOption.PreserveChanges);
        state.CaseCollection.CurrentScope.Content.AcceptChanges();
    }

    public string Display => $"Merging with '{MergedScope}'";
}
