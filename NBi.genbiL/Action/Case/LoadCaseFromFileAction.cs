using System;
using System.Linq;
using NBi.Core.FlatFile;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case;

public class LoadCaseFromFileAction : ISingleCaseAction
{
    public string Filename { get; set; }
    public LoadCaseFromFileAction(string filename)
    {
        Filename = filename;
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public virtual void Execute(CaseSet testCases)
    {
        var csvReader = new CsvReader();
        testCases.Content = csvReader.ToDataTable(Filename, true);
        testCases.Content.AcceptChanges();
    }

    public virtual string Display => $"Loading TestCases from CSV file '{Filename}'";
}
