using System;
using System.Linq;

namespace NBi.GenbiL.Action.Suite
{
    public class GenerateSuiteAction : ISuiteAction
    {
        public void Execute(GenerationState state)
        {
            state.List.Build(state.Template.Code, state.TestCases.Variables.ToArray(), state.TestCases.Content, false);
            state.Suite.DefineSettings(state.Settings.GetSettings());
            state.Suite.DefineTests(state.List.GetTests());
        }
    }
}
