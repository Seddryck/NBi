using System;
using System.Linq;
using NBi.Service;
using NBi.GenbiL.Stateful;
using NBi.Core;

namespace NBi.GenbiL.Action.Case
{
    class SaveCaseAction : ICaseAction
    {
        public string Filename { get; set; }

        public SaveCaseAction(string filename)
        {
            Filename = filename;
        }
        public void Execute(GenerationState state)
        {
            var csvWriter = new CsvWriter(true);
            csvWriter.Write(state.TestCaseSetCollection.Scope.Content, Filename);
        }

        public virtual string Display
        {
            get
            {
                return string.Format("Saving the test cases as '{0}'", Filename);
            }
        }
    }
}
