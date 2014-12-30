using NBi.GenbiL.Stateful;
using System;
using System.Linq;

namespace NBi.GenbiL.Action.Suite
{
    public class GenerateSuiteAction : ISuiteAction
    {
        public bool Grouping { get; set; }

        public GenerateSuiteAction(bool grouping)
        {
            Grouping = grouping;
        }

        public void Execute(GenerationState state)
        {
            state.List.Build(state.Template.Code, state.TestCaseCollection.Scope.Variables.ToArray(), state.TestCaseCollection.Scope.Content, Grouping);
            state.Suite.DefineSettings(state.Settings.GetSettings());
            state.Suite.DefineTests(state.List.GetTests());
        }

        public string Display
        {
            get
            {
                return string.Format("Generating Tests {0} grouping option"
                    , Grouping ? "with" : "without"
                    );
            }
        }
    }
}
