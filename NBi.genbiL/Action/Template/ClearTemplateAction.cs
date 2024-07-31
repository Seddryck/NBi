using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Template
{
    public class ClearTemplateAction : ITemplateAction
    {
        public LoadType LoadType { get; set; }
        public string Filename { get; set; } = string.Empty;
        public ClearTemplateAction()
            : base()
        {
        }

        public void Execute(GenerationState state)
        {
            state.Templates.Clear();
        }
        
        public string Display
        {
            get
            {
                return string.Format("Clearing templates");
            }
        }
    }
}
