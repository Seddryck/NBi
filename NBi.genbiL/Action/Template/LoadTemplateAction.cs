using System;
using System.Linq;
using NBi.GenbiL.Action.Case;

namespace NBi.GenbiL.Action.Template
{
    public class LoadTemplateAction : LoadCaseAction
    {
        public LoadType LoadType { get; set; }
        public LoadTemplateAction(LoadType loadType, string filename)
            : base(filename)
        {
            LoadType = loadType;
        }

        public override void Execute(GenerationState state)
        {
            Action<GenerationState> function = null;
            switch (LoadType)
            {
                case LoadType.File:
                    function = LoadExternal;
                    break;
                case LoadType.Predefined:
                    function = LoadPredefined;
                    break;
                default:
                    break;
            }
            function.Invoke(state);
        }

        private void LoadExternal(GenerationState state)
        {
            state.Template.GetExternalTemplate(Filename);
        }

        private void LoadPredefined(GenerationState state)
        {
            state.Template.GetEmbeddedTemplate(Filename);
        }
    }
}
