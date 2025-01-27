using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case;

class CopyCaseAction : IMultiCaseAction
{
    public string From { get; set; }
    public string To { get; set; }

    public CopyCaseAction(string from, string to)
    {
        From = from;
        To = to;
    }

    public void Execute(GenerationState state)
    {
        if (!state.CaseCollection.ContainsKey(From))
            throw new ArgumentException($"The set of test-cases named '{From}' doesn't exist.", nameof(From));

        if (state.CaseCollection.ContainsKey(To))
            throw new ArgumentException($"The set of test-cases named '{To}' already exists. The copy command cannot be performed on an existing test cases set", nameof(To));

        var dataReader = state.CaseCollection[From].Content.CreateDataReader();

        state.CaseCollection.Add(To, new CaseSet());
        state.CaseCollection[To].Content.Clear();
        state.CaseCollection[To].Content.Load(dataReader, LoadOption.PreserveChanges);
        state.CaseCollection[To].Content.AcceptChanges();
    }

    public virtual string Display => $"Copying set of test-cases '{From}' to '{To}'";
}
