using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Suite
{
    public class AddRangeSuiteAction : ISuiteAction
    {
        public string Filename { get; set; }

        public AddRangeSuiteAction(string filename)
        {
            Filename = filename;
        }
        
        public void Execute(GenerationState state)
        {
            state.List.AddRange(Filename);
        }

        public string Display
        {
            get
            {
                return string.Format("Include test from '{0}'"
                    , Filename
                    );
            }
        }
    }
}
