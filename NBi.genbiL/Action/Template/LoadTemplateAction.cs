using System;
using System.Linq;
using NBi.GenbiL.Action.Case;

namespace NBi.GenbiL.Action.Template
{
    public class LoadTemplateAction : ITemplateAction
    {
        public LoadType LoadType { get; set; }
        public string Filename { get; set; }
        public LoadTemplateAction(LoadType loadType, string filename)
            : base()
        {
            LoadType = loadType;
            Filename = filename;
        }

        public void Execute(GenerationState state)
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

        public string Display
        {
            get
            {
                return string.Format("Loading Template from {0} '{1}'"
                    , LoadType == Action.LoadType.File ? "file" : "predefined"
                    , Filename
                    );
            }
        }
    }
}
