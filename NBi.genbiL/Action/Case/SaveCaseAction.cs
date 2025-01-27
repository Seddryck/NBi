using System;
using System.Linq;
using NBi.Core.FlatFile;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case;

class SaveCaseAction : ISingleCaseAction
{
    public string Filename { get; set; }

    public SaveCaseAction(string filename)
    {
        Filename = filename;
    }
    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {
        var csvWriter = new CsvWriter(true);
        csvWriter.Write(testCases.Content, Filename);
    }

    public virtual string Display
    {
        get
        {
            return string.Format("Saving the test cases into '{0}'", Filename);
        }
    }
}
