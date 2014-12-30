using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Suite
{
    public class SaveSuiteAction : ISuiteAction
    {
        public string Filename { get; set; }

        public SaveSuiteAction(string filename)
        {
            Filename = filename;
        }
        
        public void Execute(GenerationState state)
        {
            state.Suite.DefineSettings(state.Settings.GetSettingsXml());
            state.Suite.DefineTests(state.List.GetTests());
            state.Suite.SaveAs(Filename);
        }

        public string Display
        {
            get
            {
                return string.Format("Saving TestSuite to '{0}'"
                    , Filename
                    );
            }
        }
    }
}
