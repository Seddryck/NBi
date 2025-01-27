using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case;

class CrossVectorCaseAction : CrossCaseAction
{
    public IEnumerable<string> Values { get; set; }

    public CrossVectorCaseAction(string firstSet, string vectorName, IEnumerable<string> values)
        : base(firstSet, vectorName)
    {
        Values = values;
    }

    public override void Execute(GenerationState state)
    {
        if (!state.CaseCollection.ContainsKey(FirstSet))
            throw new ArgumentException($"The set of test-cases named '{FirstSet}' doesn't exist.", nameof(FirstSet));

        var vector = new DataTable();
        vector.Columns.Add(SecondSet);
        foreach (var item in Values)
        {
            var row = vector.NewRow();
            row.ItemArray = new[] { item };
            vector.Rows.Add(row);
        }

        Cross(
            state.CaseCollection[FirstSet].Content,
            vector,
            state.CaseCollection.CurrentScope,
            MatchingRow);
    }

    public override bool MatchingRow(DataRow first, DataRow second) => true;

    public override string Display
        => $"Crossing set of test-cases '{FirstSet}' with vector '{SecondSet}' defined as '{string.Join("', '", Values)}'";
}
