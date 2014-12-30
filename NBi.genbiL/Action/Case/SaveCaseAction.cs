using System;
using System.Linq;
using NBi.Service;
using NBi.GenbiL.Stateful;

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
            state.TestCaseCollection.Scope.Save(Filename);
        }

        public virtual string Display
        {
            get
            {
                return string.Format("Saving the test cases into '{0}'", Filename);
            }
        }
    }
}
