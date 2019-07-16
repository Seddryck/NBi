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
            state.List.Build(
                    state.Templates, 
                    state.TestCaseCollection.CurrentScope.Variables.ToArray(), 
                    state.TestCaseCollection.CurrentScope.Content, 
                    Grouping,
                    state.Consumables
            );
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
