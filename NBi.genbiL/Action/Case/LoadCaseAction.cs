using System;
using System.Linq;
using NBi.Service;

namespace NBi.GenbiL.Action.Case
{
    public class LoadCaseAction : ICaseAction
    {
        public string Filename { get; set; }
        public LoadCaseAction(string filename)
        {
            Filename = filename;
        }

        public virtual void Execute(GenerationState state)
        {
            state.TestCases.ReadFromCsv(Filename);
        }

    }
}
